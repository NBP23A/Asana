using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ToDoServiceProxy _toDoSvc;

        public MainPageViewModel()
        {
            _toDoSvc = ToDoServiceProxy.Current;
            SortOptions = new List<string> { "Name A-Z", "Name Z-A", "Due Date Asc", "Due Date Desc" };
            SelectedSortOption = SortOptions[0];
        }

        public ToDoDetailViewModel SelectedToDo { get; set; }
        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                var filtered = _toDoSvc.ToDos
                        .Where(t => string.IsNullOrWhiteSpace(SearchText) ||
                                    t.Name?.ToLower().Contains(SearchText.ToLower()) == true)
                        .Select(t => new ToDoDetailViewModel(t));

                if (!IsShowCompleted)
                {
                    filtered = filtered.Where(t => !t?.Model?.IsCompleted ?? false);
                }

                filtered = SelectedSortOption switch
                {
                    "Name A-Z" => filtered.OrderBy(t => t.Model?.Name),
                    "Name Z-A" => filtered.OrderByDescending(t => t.Model?.Name),
                    "Due Date Asc" => filtered.OrderBy(t => t.Model?.DueDate),
                    "Due Date Desc" => filtered.OrderByDescending(t => t.Model?.DueDate),
                    _ => filtered
                };

                return new ObservableCollection<ToDoDetailViewModel>(filtered);
            }
        }

        public int SelectedToDoId => SelectedToDo?.Model?.Id ?? 0;

        private bool isShowCompleted;
        public bool IsShowCompleted
        {
            get => isShowCompleted;
            set
            {
                if (isShowCompleted != value)
                {
                    isShowCompleted = value;
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public List<string> SortOptions { get; set; }

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public void DeleteToDo()
        {
            if (SelectedToDo == null)
            {
                return;
            }

            ToDoServiceProxy.Current.DeleteToDo(SelectedToDo?.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(ToDos));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}