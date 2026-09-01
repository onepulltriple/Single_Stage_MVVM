using Microsoft.Extensions.DependencyInjection;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;
using System.Collections.ObjectModel;

namespace SingleStage.ViewModels
{
    // manages the show-management screen as a whole
    // owns the list of shows
    // owns the currently selected row
    // owns the commands for the screen
    // responds to CRUD button clicks
    // check validity in relation to other shows
    public class ManageShowsViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ShowDAC _showDAC;
        private readonly ShowScheduleValidator _showScheduleValidator;

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

        private string _scheduleErrorMessage = string.Empty;
        public string ScheduleErrorMessage
        {
            get => _scheduleErrorMessage;
            private set
            {
                if (_scheduleErrorMessage == value)
                    return;

                _scheduleErrorMessage = value;
                OnPropertyChanged(nameof(ScheduleErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // combined error message from this and editor
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(ScheduleErrorMessage))
                    return ScheduleErrorMessage;

                return Editor.ErrorMessage;
            }
        }



        public ShowEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }
        public AsyncRelayCommand ManagePerformancesCommand { get; }

        public ManageShowsViewModel(ShowDAC showDAC, IServiceProvider serviceProvider, ShowScheduleValidator showScheduleValidator)
        {
            ArgumentNullException.ThrowIfNull(showDAC);
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(showScheduleValidator);
            _showDAC = showDAC;
            _serviceProvider = serviceProvider;
            _showScheduleValidator = showScheduleValidator;

            Editor = new ShowEditorViewModel();

            // listen for editor changes so command states update when validation state changes
            Editor.PropertyChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };

            CreateCommand = new RelayCommand(_ => CreateShow(), CanCreateShow);
            EditCommand = new RelayCommand(_ => EditShow(), CanEditShow);
            SaveCommand = new AsyncRelayCommand(_ => SaveShow(), CanSaveShow);
            DeleteCommand = new AsyncRelayCommand(_ => DeleteShow(), CanDeleteShow);
            CancelCommand = new RelayCommand(_ => CancelEdit(), CanCancelEdit);

            // new command to open the ManagePerformances dialog (saves editor if needed)
            ManagePerformancesCommand = new AsyncRelayCommand(async _ => await ManagePerformancesAsync(), _ => CanManagePerformances(null));
        }

        // loads the list
        public async Task InitialiseAsync()
        {
            var shows = await _showDAC.GetAllAsync();

            ListOfShows.Clear();

            foreach (Show show in shows.OrderBy(s => s.StartTime))
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

        private async Task SaveShow()
        {
            await SaveEditorIfNeededAsync();
        }

        private async Task<Show?> SaveEditorIfNeededAsync()
        {
            // clear the previous schedule error
            ScheduleErrorMessage = string.Empty;

            // if not editing, nothing to save — return currently selected show (may be null)
            if (!Editor.IsEditing)
                return SelectedShow;

            // guard: require working copy and valid parsed times, etc.
            if (Editor.WorkingCopyShow is null || !Editor.IsValid)
                return null; // invalid => caller will abort

            // check whether the show overlaps with another show
            Show? conflictingShow = _showScheduleValidator.GetConflict(Editor.WorkingCopyShow, ListOfShows);

            if (conflictingShow is not null)
            {
                ScheduleErrorMessage =
                    $"The show overlaps with \"{conflictingShow.Name}\" " +
                    $"({conflictingShow.StartTime:ddd MMM d HH:mm} - " +
                    $"{conflictingShow.EndTime:ddd MMM d HH:mm}).";

                return null;
            }

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

            // capture the id of the saved entity, refresh the list, then find it
            var savedId = Editor.WorkingCopyShow.Id;

            await InitialiseAsync();

            var saved = ListOfShows.FirstOrDefault(s => s.Id == savedId);

            Editor.Cancel();

            SelectedShow = saved;

            UpdateCommandStates();

            return saved;
        }

        private async Task DeleteShow()
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


        private async Task ManagePerformancesAsync()
        {
            // if editing, try save first
            if (Editor.IsEditing)
            {
                var saved = await SaveEditorIfNeededAsync();
                if (saved is null)
                    return; // invalid => do not proceed
            }

            // must have a selected show to manage
            if (SelectedShow is null)
                return;

            // resolve and show the ManagePerformancesWindow via DI
            var window = _serviceProvider.GetRequiredService<Windows.ManagePerformancesWindow>();
            // set owner to main window if available so it behaves like a dialog
            window.Owner = System.Windows.Application.Current?.MainWindow;
            window.ShowDialog();
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
            // require working copy and valid parsed times, etc.
            return Editor.WorkingCopyShow is not null && Editor.IsValid;
        }

        private bool CanDeleteShow(object? parameter)
        {
            return SelectedShow is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return Editor.IsEditing;
        }

        private bool CanManagePerformances(object? parameter)
        {
            // allow if a show is selected, or if we are editing and the editor is valid
            return SelectedShow is not null || (Editor.IsEditing && Editor.IsValid);
        }

        private void UpdateCommandStates()
        {
            CreateCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();

            ManagePerformancesCommand.RaiseCanExecuteChanged();
        }
    }
}