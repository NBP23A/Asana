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
            Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            FilteredProjects = new ObservableCollection<Project>(Projects);

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

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    ApplyFilter();
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

        private ObservableCollection<Project> _projects = new();
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            private set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
                ApplyFilter();
            }
        }

        private ObservableCollection<Project> _filteredProjects = new();
        public ObservableCollection<Project> FilteredProjects
        {
            get => _filteredProjects;
            set
            {
                _filteredProjects = value;
                OnPropertyChanged(nameof(FilteredProjects));
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

                Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            }
        }

        private void DoDelete()
        {
            if (SelectedProject != null)
            {
                ProjectServiceProxy.Current.DeleteProject(SelectedProject.Id);
                SelectedProject = null;

                Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            }
        }

        private void ApplyFilter()
        {
            if (Projects == null)
                return;

            var filtered = Projects
                .Where(p => string.IsNullOrWhiteSpace(SearchText) || p.Name?.ToLower().Contains(SearchText.ToLower()) == true)
                .ToList();

            FilteredProjects = new ObservableCollection<Project>(filtered);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
