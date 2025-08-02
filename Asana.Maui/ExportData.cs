using Asana.Library.Models;
using System.Collections.Generic;

namespace Asana.Maui
{
    public class ExportData
    {
        public List<ToDo> ToDos { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
    }
}
