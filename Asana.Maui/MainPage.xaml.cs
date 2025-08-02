using Asana.Library.Models;
using Asana.Library.Services;
using Asana.Maui.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace Asana.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private async void AddNewClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ToDoDetails");
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedToDoId ?? 0;
            await Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteToDo();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainPageViewModel)?.RefreshPage();
        }

        private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e){}

        private void InLineDeleteClicked(object sender, EventArgs e)
        {
            var vm = BindingContext as MainPageViewModel;
            if (sender is Button button && button.BindingContext is ToDo toDo)
            {
                vm?.DeleteToDoById(toDo.Id);
            }
            vm?.RefreshPage();
        }

        private async void ProjectClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ProjectPage");
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            try
            {
                await ImportExportUtils.ExportDataAsync(ProjectServiceProxy.Current, ToDoServiceProxy.Current);
                await DisplayAlert("Success", "Export completed successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Export Failed", ex.Message, "OK");
            }
        }

        private async void OnImportClicked(object sender, EventArgs e)
        {
            try
            {
                await ImportExportUtils.ImportDataAsync(ProjectServiceProxy.Current, ToDoServiceProxy.Current);
                await DisplayAlert("Success", "Import completed successfully.", "OK");
                (BindingContext as MainPageViewModel)?.RefreshPage();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Import Failed", ex.Message, "OK");
            }
        }
    }
}
