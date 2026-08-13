using System.Collections.ObjectModel;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;

// manages the ticketholder-management screen as a whole
// owns the list of ticketholders
// owns the currently selected row
// owns the commands for the screen
// responds to CRUD button clicks
namespace SingleStage.ViewModels
{
    public class ManageTicketholdersViewModel : ViewModelBase
    {
        private readonly TicketholderDAC _ticketholderDAC;

        public ObservableCollection<Ticketholder> ListOfTicketholders { get; } = new();

        private Ticketholder? _selectedTicketholder;
        public Ticketholder? SelectedTicketholder
        {
            get => _selectedTicketholder;
            set
            {
                if (_selectedTicketholder == value)
                    return;

                _selectedTicketholder = value;
                OnPropertyChanged(nameof(SelectedTicketholder));

                // changing the selected ticketholder cancels any changes currently being made
                Editor.Cancel();

                EditCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
            }
        }

        public TicketholderEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ManageTicketholdersViewModel(TicketholderDAC ticketholderDAC)
        {
            ArgumentNullException.ThrowIfNull(ticketholderDAC);
            _ticketholderDAC = ticketholderDAC;

            Editor = new TicketholderEditorViewModel();

            CreateCommand   = new RelayCommand(CreateTicketholder);
            EditCommand     = new RelayCommand(_ => EditTicketholder(),     CanEditTicketholder);
            SaveCommand     = new RelayCommand(_ => SaveTicketholder(),     CanSaveTicketholder);
            DeleteCommand   = new RelayCommand(_ => DeleteTicketholder(),   CanDeleteTicketholder);
            CancelCommand   = new RelayCommand(_ => CancelEdit(),           CanCancelEdit);
        }

        // loads the list
        public async Task InitialiseAsync()
        {
            var ticketholders = await _ticketholderDAC.GetAllAsync();

            ListOfTicketholders.Clear();

            foreach (Ticketholder ticketholder in ticketholders)
            {
                ListOfTicketholders.Add(ticketholder);
            }
        }

        private void CreateTicketholder()
        {
            Editor.BeginCreate();

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private void EditTicketholder()
        {
            if (SelectedTicketholder is null)
                return;

            Editor.BeginEdit(SelectedTicketholder);

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private async void SaveTicketholder()
        {
            if (Editor.WorkingCopyTicketholder is null)
                return;

            if (string.IsNullOrWhiteSpace(Editor.WorkingCopyTicketholder.Name))
                return;

            if (Editor.WorkingCopyTicketholder.Id == 0)
            {
                // new ticketholder
                await _ticketholderDAC.AddAsync(Editor.WorkingCopyTicketholder);
            }
            else
            {
                // existing ticketholder
                await _ticketholderDAC.UpdateAsync(Editor.WorkingCopyTicketholder);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedTicketholder = null;
        }

        private async void DeleteTicketholder()
        {
            if (SelectedTicketholder is null)
                return;

            await _ticketholderDAC.DeleteAsync(SelectedTicketholder.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedTicketholder = null;
        }

        private void CancelEdit()
        {
            Editor.Cancel();

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private bool CanEditTicketholder(object? parameter)
        {
            return SelectedTicketholder is not null;
        }

        private bool CanSaveTicketholder(object? parameter)
        {
            return Editor.WorkingCopyTicketholder is not null;
        }

        private bool CanDeleteTicketholder(object? parameter)
        {
            return SelectedTicketholder is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return Editor.IsEditing;
        }
    }
}
