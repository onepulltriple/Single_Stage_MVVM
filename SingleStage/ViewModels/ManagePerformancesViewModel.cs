using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SingleStage.ViewModels
{
    public class ManagePerformancesViewModel : ViewModelBase
    {
        private readonly PerformanceDAC _performanceDAC;
        private readonly ShowDAC _showDAC;
        private readonly PerformanceScheduleValidator _performanceScheduleValidator;

        private readonly List<Performance> _allPerformances = new();
        public ObservableCollection<Show> ListOfShows { get; } = new();
        public ObservableCollection<Performance> ListOfPerformances { get; } = new();
        //public ObservableCollection<Artist> ListOfArtists { get; } = new();

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
                PerformanceEditor.Cancel();

                UpdateCommandStates();
            }
        }

        private int? _filterByShowId;
        public int? FilterByShowId
        {
            get => _filterByShowId;
            set
            {
                if (_filterByShowId == value)
                    return;

                _filterByShowId = value;
                OnPropertyChanged(nameof(FilterByShowId));

                ScheduleErrorMessage = string.Empty; // clear schedule error

                PerformanceEditor.Cancel();
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

                return PerformanceEditor.ErrorMessage;
            }
        }


        public PerformanceEditorViewModel PerformanceEditor { get; }
        public ArtistPerformanceEditorViewModel ArtistPerformanceEditor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ListAllCommand { get; }

        #region management mode
        public enum PerformanceManagementMode
        {
            Performances,
            ArtistPerformances
        }

        private PerformanceManagementMode _managementMode = PerformanceManagementMode.Performances;
        // this default makes InManageArtistPerformancesMode return false
        public PerformanceManagementMode ManagementMode
        {
            get => _managementMode;
            set
            {
                if (_managementMode == value)
                    return;

                _managementMode = value;

                PerformanceEditor.Cancel();
                ArtistPerformanceEditor.Cancel();

                ScheduleErrorMessage = string.Empty;

                //SelectedPerformance = null; // selected performance survives the mode switch
                //SelectedArtistPerformance = null;

                OnPropertyChanged(nameof(ManagementMode));
                OnPropertyChanged(nameof(InManageArtistPerformancesMode));
                OnPropertyChanged(nameof(RowDetailsVisibilityMode));

                UpdateCommandStates();
            }
        }

        public bool InManageArtistPerformancesMode
        {
            get => ManagementMode == PerformanceManagementMode.ArtistPerformances;
            set => ManagementMode = value
                ? PerformanceManagementMode.ArtistPerformances // true --> show ArtistPerformanceEditorView
                : PerformanceManagementMode.Performances; // false --> show PerformanceEditorView
        }

        public DataGridRowDetailsVisibilityMode RowDetailsVisibilityMode
        {
            get => InManageArtistPerformancesMode
                ? DataGridRowDetailsVisibilityMode.Visible
                : DataGridRowDetailsVisibilityMode.Collapsed;
        }

        #endregion

        public ManagePerformancesViewModel(PerformanceDAC performanceDAC, ShowDAC showDAC, PerformanceScheduleValidator performanceScheduleValidator)
        {
            ArgumentNullException.ThrowIfNull(performanceDAC);
            ArgumentNullException.ThrowIfNull(showDAC);
            ArgumentNullException.ThrowIfNull(performanceScheduleValidator);

            _performanceDAC = performanceDAC;
            _showDAC = showDAC;
            _performanceScheduleValidator = performanceScheduleValidator;

            PerformanceEditor = new PerformanceEditorViewModel();
            ArtistPerformanceEditor = new ArtistPerformanceEditorViewModel();

            PerformanceEditor.PropertyChanged += (_, _) =>
            {
                // the schedule error should persist even while the user modifies the editor
                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };
            ArtistPerformanceEditor.PropertyChanged += (_, _) =>
            {
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
            int? currentShowId = FilterByShowId;

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

            // restore the previous filter if that show still exists.
            if (currentShowId.HasValue &&
                ListOfShows.Any(show => show.Id == currentShowId.Value))
            {
                _filterByShowId = currentShowId;
                OnPropertyChanged(nameof(FilterByShowId));
            }
            else if (currentShowId.HasValue)
            {
                // the previously selected show no longer exists.
                _filterByShowId = null;
                OnPropertyChanged(nameof(FilterByShowId));
            }

            ApplyPerformanceFilter();
        }

        private void ApplyPerformanceFilter()
        {
            IEnumerable<Performance> performances = _allPerformances;

            if (FilterByShowId.HasValue)
            {
                performances = performances.Where(
                    performance => performance.ShowId == FilterByShowId.Value);
            }

            ListOfPerformances.Clear();

            foreach (Performance performance in performances.OrderBy(performance => performance.StartTime))
            {
                ListOfPerformances.Add(performance);
            }
        }

        private void ListAllPerformances()
        {
            FilterByShowId = null;
        }

        private void CreatePerformance()
        {
            ScheduleErrorMessage = string.Empty;

            if (!FilterByShowId.HasValue)
            {
                PerformanceEditor.BeginCreate();

                UpdateCommandStates();

                return;
            }

            Show? selectedShow = ListOfShows.FirstOrDefault(
                show => show.Id == FilterByShowId.Value);

            if (selectedShow is null)
            {
                PerformanceEditor.BeginCreate();

                UpdateCommandStates();

                return;
            }

            var slot = FindFirstAvailablePerformanceSlot(selectedShow);

            if (slot is null)
            {
                ScheduleErrorMessage = "There is no available time remaining in the selected show.";
                UpdateCommandStates();
                return;
            }

            PerformanceEditor.BeginCreate(
                slot.Value.Start,
                slot.Value.End,
                selectedShow.Id);

            UpdateCommandStates();
        }

        private (DateTime Start, DateTime End)? FindFirstAvailablePerformanceSlot(Show show)
        {
            DateTime currentTime = show.StartTime;

            var showPerformances = _allPerformances
                .Where(performance => performance.ShowId == show.Id)
                .OrderBy(performance => performance.StartTime)
                .ToList();

            foreach (Performance performance in showPerformances)
            {
                if (performance.EndTime <= currentTime)
                {
                    continue;
                }

                if (performance.StartTime > currentTime)
                {
                    return (currentTime, performance.StartTime);
                }

                if (performance.EndTime > currentTime)
                {
                    currentTime = performance.EndTime;
                }
            }

            if (currentTime < show.EndTime)
            {
                return (currentTime, show.EndTime);
            }

            return null;
        }

        private void EditPerformance()
        {
            if (SelectedPerformance is null) 
                return;

            ScheduleErrorMessage = string.Empty;

            PerformanceEditor.BeginEdit(SelectedPerformance);

            UpdateCommandStates();
        }

        private async Task SavePerformance()
        {
            int savedPerformanceId;

            ScheduleErrorMessage = string.Empty;

            if (PerformanceEditor.WorkingCopyPerformance is null || !PerformanceEditor.IsValid)
                return;

            string? validationError = _performanceScheduleValidator.Validate(
                PerformanceEditor.WorkingCopyPerformance,
                ListOfPerformances,
                ListOfShows);

            if (validationError is not null)
            {
                ScheduleErrorMessage = validationError;
                return;
            }

            if (PerformanceEditor.WorkingCopyPerformance.Id == 0)
            {
                await _performanceDAC.AddAsync(PerformanceEditor.WorkingCopyPerformance);
            }
            else
            {
                await _performanceDAC.UpdateAsync(PerformanceEditor.WorkingCopyPerformance);
            }
            
            savedPerformanceId = PerformanceEditor.WorkingCopyPerformance.Id;

            await InitialiseAsync();

            PerformanceEditor.Cancel();

            SelectedPerformance = ListOfPerformances.FirstOrDefault(
                performance => performance.Id == savedPerformanceId);

            UpdateCommandStates();
        }


        private async Task DeletePerformance()
        {
            if (SelectedPerformance is null) 
                return;

            await _performanceDAC.DeleteAsync(SelectedPerformance.Id);

            await InitialiseAsync();

            PerformanceEditor.Cancel();

            SelectedPerformance = null;

            UpdateCommandStates();
        }

        private void CancelEdit()
        {
            ScheduleErrorMessage = string.Empty;

            PerformanceEditor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreatePerformance(object? parameter)
        {
            return !PerformanceEditor.IsEditing;
        } 

        private bool CanEditPerformance(object? parameter)
        {
            return SelectedPerformance is not null;
        } 

        private bool CanSavePerformance(object? parameter)
        {
            // require working copy and valid parsed times, etc.
            return PerformanceEditor.WorkingCopyPerformance is not null && PerformanceEditor.IsValid;
        }

        private bool CanDeletePerformance(object? parameter)
        {
            return SelectedPerformance is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return PerformanceEditor.IsEditing;
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