using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;

namespace SingleStage.ViewModels
{
    // manages the show-management screen as a whole
    // owns the list of shows
    // owns the currently selected row
    // owns the commands for the screen
    // responds to CRUD button clicks
    public class ManageShowsViewModel : ViewModelBase
    {
        private readonly ShowDAC _showDAC;

        public ObservableCollection<Show> ListOfShows { get; } = new();

        private Show? _selectedShow;
        public Show? SelectedShow
        {
            get => _selectedShow;
            set
            {
                if (_selectedShow == value)
                    return;

                _selectedShow = value;
                OnPropertyChanged(nameof(SelectedShow));

                // changing the selected show cancels any changes currently being made
                Editor.Cancel();

                UpdateCommandStates();
            }
        }

        public ShowEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ManageShowsViewModel(ShowDAC showDAC)
        {
            ArgumentNullException.ThrowIfNull(showDAC);
            _showDAC = showDAC;

            Editor = new ShowEditorViewModel();

            CreateCommand = new RelayCommand(_ => CreateShow(), CanCreateShow);
            EditCommand = new RelayCommand(_ => EditShow(), CanEditShow);
            SaveCommand = new RelayCommand(_ => SaveShow(), CanSaveShow);
            DeleteCommand = new RelayCommand(_ => DeleteShow(), CanDeleteShow);
            CancelCommand = new RelayCommand(_ => CancelEdit(), CanCancelEdit);
        }

        // loads the list
        public async Task InitialiseAsync()
        {
            var shows = await _showDAC.GetAllAsync();

            ListOfShows.Clear();

            foreach (Show show in shows)
            {
                ListOfShows.Add(show);
            }
        }

        private void CreateShow()
        {
            Editor.BeginCreate();

            UpdateCommandStates();
        }

        private void EditShow()
        {
            if (SelectedShow is null)
                return;

            Editor.BeginEdit(SelectedShow);

            UpdateCommandStates();
        }

        private async void SaveShow()
        {
            if (Editor.WorkingCopyShow is null)
                return;

            if (string.IsNullOrWhiteSpace(Editor.WorkingCopyShow.Name))
                return;

            if (Editor.WorkingCopyShow.Id == 0)
            {
                // new show
                await _showDAC.AddAsync(Editor.WorkingCopyShow);
            }
            else
            {
                // existing show
                await _showDAC.UpdateAsync(Editor.WorkingCopyShow);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedShow = null;

            UpdateCommandStates();
        }

        private async void DeleteShow()
        {
            if (SelectedShow is null)
                return;

            await _showDAC.DeleteAsync(SelectedShow.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedShow = null;

            UpdateCommandStates();
        }

        private void CancelEdit()
        {
            Editor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreateShow(object? arg)
        {
            return !Editor.IsEditing;
        }

        private bool CanEditShow(object? parameter)
        {
            return SelectedShow is not null;
        }

        private bool CanSaveShow(object? parameter)
        {
            return Editor.WorkingCopyShow is not null;
        }

        private bool CanDeleteShow(object? parameter)
        {
            return SelectedShow is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return Editor.IsEditing;
        }

        private void UpdateCommandStates()
        {
            CreateCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }
    }
}