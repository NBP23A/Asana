using Asana.Library.Models;
using Asana.Library.Services;
using Microsoft.Maui.Storage;
using System.Text.Json;
using Asana.Maui;

namespace Asana.Maui
{
    public static class ImportExportUtils
    {
        public static async Task ExportDataAsync(ProjectServiceProxy projectService, ToDoServiceProxy toDoService)
        {
            var exportData = new ExportData
            {
                Projects = projectService.Projects,
                ToDos = toDoService.ToDos
            };

            var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });

            var fileName = $"asana_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllTextAsync(filePath, json);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export Asana Data",
                File = new ShareFile(filePath)
            });
        }

        public static async Task ImportDataAsync(ProjectServiceProxy projectService, ToDoServiceProxy toDoService)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.iOS, new[] { "public.json" } }
                }),
                PickerTitle = "Select a JSON file to import"
            });

            if (result == null)
                return;

            var json = await File.ReadAllTextAsync(result.FullPath);
            var importData = JsonSerializer.Deserialize<ExportData>(json);

            if (importData == null)
                return;

            foreach (var project in importData.Projects)
            {
                projectService.AddOrUpdateProject(project);
            }

            foreach (var todo in importData.ToDos)
            {
                toDoService.AddOrUpdate(todo);
            }
        }
    }
}
