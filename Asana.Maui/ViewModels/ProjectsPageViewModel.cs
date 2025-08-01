using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ProjectsPageViewModel()
        {
            _projects = ProjectServiceProxy.Current.Projects;
            AddCommand = new Command(DoAdd);
            DeleteCommand = new Command(DoDelete);
            SortOptions = Enum.GetValues(typeof(ProjectSortOption)).Cast<ProjectSortOption>().ToList();
            SelectedSortOption = ProjectSortOption.NameAscending;
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

        private List<Project> _sortedProjects = new();
        public List<Project> SortedProjects
        {
            get => _sortedProjects;
            set
            {
                _sortedProjects = value;
                OnPropertyChanged(nameof(SortedProjects));
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public List<ProjectSortOption> SortOptions { get; set; }

        private ProjectSortOption _selectedSortOption;
        public ProjectSortOption SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged(nameof(SelectedSortOption));
                    SortProjects();
                }
            }
        }

        private void DoAdd()
        {
            if (!string.IsNullOrWhiteSpace(NewProjectName))
            {
                var newProject = new Project { Name = NewProjectName };
                ProjectServiceProxy.Current.AddOrUpdateProject(newProject);
                NewProjectName = string.Empty;
                Projects = ProjectServiceProxy.Current.Projects;
                SortProjects();
            }
        }

        private void DoDelete()
        {
            if (SelectedProject != null)
            {
                ProjectServiceProxy.Current.DeleteProject(SelectedProject.Id);
                Projects = ProjectServiceProxy.Current.Projects;
                SelectedProject = null;
                SortProjects();
            }
        }

        private void SortProjects()
        {
            switch (SelectedSortOption)
            {
                case ProjectSortOption.NameAscending:
                    SortedProjects = Projects.OrderBy(p => p.Name).ToList();
                    break;
                case ProjectSortOption.NameDescending:
                    SortedProjects = Projects.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    SortedProjects = Projects;
                    break;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum ProjectSortOption
    {
        NameAscending,
        NameDescending
    }
}