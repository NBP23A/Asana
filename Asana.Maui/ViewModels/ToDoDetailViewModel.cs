using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel : INotifyPropertyChanged
    {
        public ToDoDetailViewModel() {
            //Model = new ToDo();
            Model = new ToDo { DueDate = DateTime.Today };

            DeleteCommand = new Command(DoDelete);

            Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            SelectedProject = Projects.FirstOrDefault(p => p.Id == Model.ProjectId);
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();

            DeleteCommand = new Command(DoDelete);

            Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            SelectedProject = Projects.FirstOrDefault(p => p.Id == Model.ProjectId);
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            //Model = model ?? new ToDo();
            Model = model ?? new ToDo { DueDate = DateTime.Today };
            DeleteCommand = new Command(DoDelete);

            Projects = new ObservableCollection<Project>(ProjectServiceProxy.Current.Projects);
            SelectedProject = Projects.FirstOrDefault(p => p.Id == Model.ProjectId);
        }

        public void DoDelete() {

            ToDoServiceProxy.Current.DeleteToDo(Model?.Id ?? 0);
        }

        public ToDo? Model { get ; set; }
        public ICommand? DeleteCommand { get; set; }

        public List<int> Priorities
        {
            get
            {
                return new List<int> { 0, 1, 2, 3, 4 };
            }
        }

        public int SelectedPriority { 
            get
            {
                return Model?.Priority ?? 4;
            }
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }

        //This is option 1 to fix the UX issue with Priority
        public string PriorityDisplay
        {
            set
            {
                if(Model == null)
                {
                    return;
                }

                if (!int.TryParse(value, out int p))
                {
                    Model.Priority = -9999;
                }
                else
                {
                    Model.Priority = p;
                }
            }

            get
            {
                return Model?.Priority?.ToString() ?? string.Empty;
            }
        }

        private Project? _selectedProject;
        public ObservableCollection<Project> Projects { get; set; }

        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    if (Model != null)
                    {
                        Model.ProjectId = value?.Id;
                    }
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
