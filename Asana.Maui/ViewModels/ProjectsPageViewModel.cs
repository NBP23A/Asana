using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ProjectsPageViewModel()
        {
            Projects = ProjectServiceProxy.Current.Projects;
            AddCommand = new Command(DoAdd);
            DeleteCommand = new Command(DoDelete);
        }

        private string? _newProjectName;
        public string? NewProjectName
        {
            get => _newProjectName;
            set
            {
                if (_newProjectName != value)
                {
                    _newProjectName = value;
                    OnPropertyChanged(nameof(NewProjectName));
                }
            }
        }

        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        private List<Project> _projects = new();
        public List<Project> Projects
        {
            get => _projects;
            private set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private void DoAdd()
        {
            if (!string.IsNullOrWhiteSpace(NewProjectName))
            {
                var newProject = new Project { Name = NewProjectName };
                ProjectServiceProxy.Current.AddOrUpdateProject(newProject);
                NewProjectName = string.Empty;
                Projects = ProjectServiceProxy.Current.Projects;
            }
        }

        private void DoDelete()
        {
            if (SelectedProject != null)
            {
                ProjectServiceProxy.Current.DeleteProject(SelectedProject.Id);
                Projects = ProjectServiceProxy.Current.Projects;
                SelectedProject = null;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}