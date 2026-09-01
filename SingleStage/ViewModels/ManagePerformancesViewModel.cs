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

        public ObservableCollection<Performance> ListOfPerformances { get; } = new();
        public ObservableCollection<Show> ListOfShows { get; } = new();

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

        public PerformanceEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ManagePerformancesViewModel(PerformanceDAC performanceDAC, ShowDAC showDAC)
        {
            ArgumentNullException.ThrowIfNull(performanceDAC);
            ArgumentNullException.ThrowIfNull(showDAC);

            _performanceDAC = performanceDAC;
            _showDAC = showDAC;

            Editor = new PerformanceEditorViewModel();

            CreateCommand = new RelayCommand(_ => CreatePerformance(), _ => CanCreatePerformance(null));
            EditCommand = new RelayCommand(_ => EditPerformance(), _ => CanEditPerformance(null));
            SaveCommand = new AsyncRelayCommand(async _ => await SavePerformance(), _ => CanSavePerformance(null));
            DeleteCommand = new AsyncRelayCommand(async _ => await DeletePerformance(), _ => CanDeletePerformance(null));
            CancelCommand = new RelayCommand(_ => CancelEdit(), _ => CanCancelEdit(null));
        }

        // loads the lists
        public async Task InitialiseAsync()
        {
            var performances = await _performanceDAC.GetAllAsync();
            var shows = await _showDAC.GetAllAsync();

            ListOfPerformances.Clear();

            foreach (Performance performance in performances.OrderBy(performance => performance.StartTime))
            {
                ListOfPerformances.Add(performance);
            }

            ListOfShows.Clear();

            foreach (Show show in shows.OrderBy(show => show.StartTime))
            {
                ListOfShows.Add(show);
            }
        }

        private void CreatePerformance()
        {
            Editor.BeginCreate();

            UpdateCommandStates();
        }

        private void EditPerformance()
        {
            if (SelectedPerformance is null) 
                return;

            Editor.BeginEdit(SelectedPerformance);

            UpdateCommandStates();
        }

        private async Task SavePerformance()
        {
            if (Editor.WorkingCopyPerformance is null || !Editor.IsValid)
                return;

            if (Editor.WorkingCopyPerformance.Id == 0)
            {
                // new performance
                await _performanceDAC.AddAsync(Editor.WorkingCopyPerformance);
            }
            else
            {
                // existing performance
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