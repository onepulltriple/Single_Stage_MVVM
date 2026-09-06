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
        private readonly ShowDAC _showDAC;
        private readonly PerformanceDAC _performanceDAC;
        private readonly ArtistPerformanceDAC _artistPerformanceDAC;
        private readonly PerformanceScheduleValidator _performanceScheduleValidator;

        private readonly List<Performance> _allPerformances = new();
        public ObservableCollection<Show> ListOfShows { get; } = new();
        public ObservableCollection<Performance> ListOfPerformances { get; } = new();
        public ObservableCollection<ArtistPerformance> ListOfArtistPerformances { get; } = new();


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

                // a different performance means any ArtistPerformance edit
                // belongs to the old performance, so abandon it
                PerformanceEditor.Cancel();
                ArtistPerformanceEditor.Cancel();

                // the selected ArtistPerformance also belongs to the old performance, so set it to null
                SelectedArtistPerformance = null;

                // repopulate the ArtistPerformance picker
                PopulateArtistPerformances();

                UpdateCommandStates();
            }
        }

        private ArtistPerformance? _selectedArtistPerformance;
        public ArtistPerformance? SelectedArtistPerformance
        {
            get => _selectedArtistPerformance;
            set
            {
                if (_selectedArtistPerformance == value)
                    return;

                _selectedArtistPerformance = value;

                OnPropertyChanged(nameof(SelectedArtistPerformance));

                // cancel editor edits when selection changes
                ArtistPerformanceEditor.Cancel();

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
                SelectedArtistPerformance = null;

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

        // constructor
        public ManagePerformancesViewModel(
            ShowDAC showDAC, 
            PerformanceDAC performanceDAC, 
            ArtistPerformanceDAC artistPerformanceDAC,
            PerformanceScheduleValidator performanceScheduleValidator)
        {
            ArgumentNullException.ThrowIfNull(showDAC);
            ArgumentNullException.ThrowIfNull(performanceDAC);
            ArgumentNullException.ThrowIfNull(artistPerformanceDAC);
            ArgumentNullException.ThrowIfNull(performanceScheduleValidator);

            _showDAC = showDAC;
            _performanceDAC = performanceDAC;
            _artistPerformanceDAC = artistPerformanceDAC;
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

            CreateCommand = 
                new RelayCommand(_ => Create(), _ => CanCreate(null));
            EditCommand = 
                new RelayCommand(_ => Edit(), _ => CanEdit(null));
            SaveCommand = 
                new AsyncRelayCommand(async _ => await Save(), _ => CanSave(null));
            DeleteCommand = 
                new AsyncRelayCommand(async _ => await Delete(), _ => CanDelete(null));
            CancelCommand = 
                new RelayCommand(_ => CancelEdit(), _ => CanCancelEdit(null));

            ListAllCommand = 
                new RelayCommand(_ => ListAllPerformances());

        }

        // load the lists
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

        private void PopulateArtistPerformances()
        {
            ListOfArtistPerformances.Clear();

            if (SelectedPerformance is null)
                return;

            foreach (ArtistPerformance artistPerformance in SelectedPerformance.ArtistPerformances
                         .OrderBy(ap => ap.Artist.Name))
            {
                ListOfArtistPerformances.Add(artistPerformance);
            }
        }


        #region dispatcher methods
        private void Create()
        {
            switch (ManagementMode)
            {
                case PerformanceManagementMode.Performances:
                    CreatePerformance();
                    break;

                case PerformanceManagementMode.ArtistPerformances:
                    CreateArtistPerformance();
                    break;
            }
        }

        private bool CanCreate(object? parameter)
        {
            return ManagementMode switch
            {
                PerformanceManagementMode.Performances =>
                    CanCreatePerformance(parameter),

                PerformanceManagementMode.ArtistPerformances =>
                    CanCreateArtistPerformance(parameter),

                _ => false
            };
        }

        private void Edit()
        {
            switch (ManagementMode)
            {
                case PerformanceManagementMode.Performances:
                    EditPerformance();
                    break;

                case PerformanceManagementMode.ArtistPerformances:
                    EditArtistPerformance();
                    break;
            }
        }

        private bool CanEdit(object? parameter)
        {
            return ManagementMode switch
            {
                PerformanceManagementMode.Performances =>
                    CanEditPerformance(parameter),

                PerformanceManagementMode.ArtistPerformances =>
                    CanEditArtistPerformance(parameter),

                _ => false
            };
        }

        private async Task Save()
        {
            switch (ManagementMode)
            {
                case PerformanceManagementMode.Performances:
                    await SavePerformance();
                    break;

                case PerformanceManagementMode.ArtistPerformances:
                    await SaveArtistPerformance();
                    break;
            }
        }

        private bool CanSave(object? parameter)
        {
            return ManagementMode switch
            {
                PerformanceManagementMode.Performances =>
                    CanSavePerformance(parameter),

                PerformanceManagementMode.ArtistPerformances =>
                    CanSaveArtistPerformance(parameter),

                _ => false
            };
        }

        private async Task Delete()
        {
            switch (ManagementMode)
            {
                case PerformanceManagementMode.Performances:
                    await DeletePerformance();
                    break;

                case PerformanceManagementMode.ArtistPerformances:
                    await DeleteArtistPerformance();
                    break;
            }
        }

        private bool CanDelete(object? parameter)
        {
            return ManagementMode switch
            {
                PerformanceManagementMode.Performances =>
                    CanDeletePerformance(parameter),

                PerformanceManagementMode.ArtistPerformances =>
                    CanDeleteArtistPerformance(parameter),

                _ => false
            };
        }


        #endregion


        #region Manage performances
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


        #endregion

        #region Manage artist performances
        private void CreateArtistPerformance()
        {
            if (SelectedPerformance is null)
                return;

            ArtistPerformanceEditor.BeginCreate(SelectedPerformance.Id);

            UpdateCommandStates();
        }

        private void EditArtistPerformance()
        {
            if (SelectedArtistPerformance is null)
                return;

            ArtistPerformanceEditor.BeginEdit(SelectedArtistPerformance);

            UpdateCommandStates();
        }

        private async Task SaveArtistPerformance()
        {
            if (ArtistPerformanceEditor.WorkingCopyArtistPerformance is null
                || !ArtistPerformanceEditor.IsValid)
                return;

            ArtistPerformance workingCopy =
                ArtistPerformanceEditor.WorkingCopyArtistPerformance;

            int savedArtistPerformanceId = workingCopy.Id;
            int performanceId = workingCopy.PerformanceId;

            if (workingCopy.Id == 0)
            {
                await _artistPerformanceDAC.AddAsync(workingCopy);
                savedArtistPerformanceId = workingCopy.Id;
            }
            else
            {
                await _artistPerformanceDAC.UpdateAsync(workingCopy);
            }

            ArtistPerformanceEditor.Cancel();

            Performance? refreshedPerformance =
                await _performanceDAC.GetByIdAsync(performanceId);

            if (refreshedPerformance is null)
            {
                SelectedPerformance = null;
                return;
            }

            Performance? oldPerformance =
                ListOfPerformances.FirstOrDefault(p => p.Id == performanceId);

            if (oldPerformance is not null)
            {
                int index = ListOfPerformances.IndexOf(oldPerformance);
                ListOfPerformances[index] = refreshedPerformance;
            }

            int allPerformancesIndex =
                _allPerformances.FindIndex(p => p.Id == performanceId);

            if (allPerformancesIndex >= 0)
            {
                _allPerformances[allPerformancesIndex] = refreshedPerformance;
            }

            SelectedPerformance = refreshedPerformance;

            PopulateArtistPerformances();

            SelectedArtistPerformance =
                ListOfArtistPerformances.FirstOrDefault(
                    ap => ap.Id == savedArtistPerformanceId);

            UpdateCommandStates();
        }


        private async Task DeleteArtistPerformance()
        {
            if (SelectedArtistPerformance is null)
                return;

            int performanceId = SelectedArtistPerformance.PerformanceId;

            await _artistPerformanceDAC.DeleteAsync(
                SelectedArtistPerformance.Id);

            ArtistPerformanceEditor.Cancel();
            SelectedArtistPerformance = null;

            Performance? refreshedPerformance =
                await _performanceDAC.GetByIdAsync(performanceId);

            if (refreshedPerformance is null)
            {
                SelectedPerformance = null;
                return;
            }

            Performance? oldPerformance =
                ListOfPerformances.FirstOrDefault(p => p.Id == performanceId);

            if (oldPerformance is not null)
            {
                int index = ListOfPerformances.IndexOf(oldPerformance);
                ListOfPerformances[index] = refreshedPerformance;
            }

            int allPerformancesIndex =
                _allPerformances.FindIndex(p => p.Id == performanceId);

            if (allPerformancesIndex >= 0)
            {
                _allPerformances[allPerformancesIndex] = refreshedPerformance;
            }

            SelectedPerformance = refreshedPerformance;

            PopulateArtistPerformances();

            UpdateCommandStates();
        }


        private bool CanCreateArtistPerformance(object? parameter)
        {
            return SelectedPerformance is not null
                && !ArtistPerformanceEditor.IsEditing;
        }

        private bool CanEditArtistPerformance(object? parameter)
        {
            return SelectedArtistPerformance is not null
                && !ArtistPerformanceEditor.IsEditing;
        }

        private bool CanSaveArtistPerformance(object? parameter)
        {
            return ArtistPerformanceEditor.WorkingCopyArtistPerformance is not null
                && ArtistPerformanceEditor.IsValid;
        }

        private bool CanDeleteArtistPerformance(object? parameter)
        {
            return SelectedArtistPerformance is not null
                && !ArtistPerformanceEditor.IsEditing;
        }


        #endregion
        
        private void CancelEdit()
        {
            ScheduleErrorMessage = string.Empty;

            switch (ManagementMode)
            {
                case PerformanceManagementMode.Performances:
                    PerformanceEditor.Cancel();
                    break;

                case PerformanceManagementMode.ArtistPerformances:
                    ArtistPerformanceEditor.Cancel();
                    break;
            }

            UpdateCommandStates();
        }

        private bool CanCancelEdit(object? parameter)
        {
            return ManagementMode switch
            {
                PerformanceManagementMode.Performances =>
                    PerformanceEditor.IsEditing,

                PerformanceManagementMode.ArtistPerformances =>
                    ArtistPerformanceEditor.IsEditing,

                _ => false
            };
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