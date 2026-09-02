using System.Collections.ObjectModel;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;

namespace SingleStage.ViewModels
{
    public class ManagePerformancesViewModel : ViewModelBase
    {
        private readonly PerformanceDAC _performanceDAC;
        private readonly ShowDAC _showDAC;
        private readonly PerformanceScheduleValidator _performanceScheduleValidator;

        public ObservableCollection<Performance> ListOfPerformances { get; } = new();
        public ObservableCollection<Show> ListOfShows { get; } = new();
        private readonly List<Performance> _allPerformances = new();

        private Performance? _selectedPerformance;
        public Performance? SelectedPerformance
        {
            get => _selectedPerformance;
            set
            {
                if (_selectedPerformance == value) 
                    return;

                _selectedPerformance = value;

                OnPropertyChanged(nameof(SelectedPerformance));

                // cancel editor edits when selection changes
                Editor.Cancel();

                UpdateCommandStates();
            }
        }

        private int? _showId;
        public int? ShowId
        {
            get => _showId;
            set
            {
                if (_showId == value)
                    return;

                _showId = value;
                OnPropertyChanged(nameof(ShowId));

                Editor.Cancel();
                SelectedPerformance = null;

                ApplyPerformanceFilter();
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

        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(ScheduleErrorMessage))
                    return ScheduleErrorMessage;

                return Editor.ErrorMessage;
            }
        }


        public PerformanceEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ListAllCommand { get; }


        public ManagePerformancesViewModel(PerformanceDAC performanceDAC, ShowDAC showDAC, PerformanceScheduleValidator performanceScheduleValidator)
        {
            ArgumentNullException.ThrowIfNull(performanceDAC);
            ArgumentNullException.ThrowIfNull(showDAC);
            ArgumentNullException.ThrowIfNull(performanceScheduleValidator);

            _performanceDAC = performanceDAC;
            _showDAC = showDAC;
            _performanceScheduleValidator = performanceScheduleValidator;

            Editor = new PerformanceEditorViewModel();
            
            Editor.PropertyChanged += (_, _) =>
            {
                //ScheduleErrorMessage = string.Empty;
                // the schedule error should persist even while the user modifies the editor
                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };

            CreateCommand = new RelayCommand(_ => CreatePerformance(), _ => CanCreatePerformance(null));
            EditCommand = new RelayCommand(_ => EditPerformance(), _ => CanEditPerformance(null));
            SaveCommand = new AsyncRelayCommand(async _ => await SavePerformance(), _ => CanSavePerformance(null));
            DeleteCommand = new AsyncRelayCommand(async _ => await DeletePerformance(), _ => CanDeletePerformance(null));
            CancelCommand = new RelayCommand(_ => CancelEdit(), _ => CanCancelEdit(null));
            ListAllCommand = new RelayCommand(_ => ListAllPerformances());

        }

        // loads the lists
        public async Task InitialiseAsync()
        {
            var performances = await _performanceDAC.GetAllAsync();
            var shows = await _showDAC.GetAllAsync();

            _allPerformances.Clear();
            foreach (Performance performance in performances)
            {
                _allPerformances.Add(performance);
            }

            ListOfShows.Clear();
            foreach (Show show in shows.OrderBy(show => show.StartTime))
            {
                ListOfShows.Add(show);
            }

            // if a ShowId was supplied but that show no longer exists,
            // fall back to showing all performances
            if (ShowId.HasValue && !ListOfShows.Any(show => show.Id == ShowId.Value))
            {
                _showId = null; // setter call not needed when initialising
                OnPropertyChanged(nameof(ShowId));
            }

            ApplyPerformanceFilter();
        }

        private void ApplyPerformanceFilter()
        {
            IEnumerable<Performance> performances = _allPerformances;

            if (ShowId.HasValue)
            {
                performances = performances.Where(
                    performance => performance.ShowId == ShowId.Value);
            }

            ListOfPerformances.Clear();

            foreach (Performance performance in performances.OrderBy(performance => performance.StartTime))
            {
                ListOfPerformances.Add(performance);
            }
        }

        private void ListAllPerformances()
        {
            ShowId = null;
        }


        private void CreatePerformance()
        {
            ScheduleErrorMessage = string.Empty;

            Editor.BeginCreate();

            if (ShowId.HasValue)
            {
                Editor.ShowId = ShowId.Value;
            }

            UpdateCommandStates();
        }

        private void EditPerformance()
        {
            if (SelectedPerformance is null) 
                return;

            ScheduleErrorMessage = string.Empty;

            Editor.BeginEdit(SelectedPerformance);

            UpdateCommandStates();
        }

        private async Task SavePerformance()
        {
            ScheduleErrorMessage = string.Empty;

            if (Editor.WorkingCopyPerformance is null || !Editor.IsValid)
                return;

            string? validationError = _performanceScheduleValidator.Validate(
                Editor.WorkingCopyPerformance,
                ListOfPerformances,
                ListOfShows);

            if (validationError is not null)
            {
                ScheduleErrorMessage = validationError;
                return;
            }

            if (Editor.WorkingCopyPerformance.Id == 0)
            {
                await _performanceDAC.AddAsync(Editor.WorkingCopyPerformance);
            }
            else
            {
                await _performanceDAC.UpdateAsync(Editor.WorkingCopyPerformance);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedPerformance = null;

            UpdateCommandStates();
        }


        private async Task DeletePerformance()
        {
            if (SelectedPerformance is null) 
                return;

            await _performanceDAC.DeleteAsync(SelectedPerformance.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedPerformance = null;

            UpdateCommandStates();
        }

        private void CancelEdit()
        {
            ScheduleErrorMessage = string.Empty;

            Editor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreatePerformance(object? parameter)
        {
            return !Editor.IsEditing;
        } 

        private bool CanEditPerformance(object? parameter)
        {
            return SelectedPerformance is not null;
        } 

        private bool CanSavePerformance(object? parameter)
        {
            // require working copy and valid parsed times, etc.
            return Editor.WorkingCopyPerformance is not null && Editor.IsValid;
        }

        private bool CanDeletePerformance(object? parameter)
        {
            return SelectedPerformance is not null;
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