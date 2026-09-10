using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.Infrastructure.EventArguments;
using SingleStage.ViewModels.EditorViewModels;
using System.Collections.ObjectModel;

namespace SingleStage.ViewModels
{
    // manages the ticketholder-management screen as a whole
    // owns the list of ticketholders
    // owns the currently selected row
    // owns the commands for the screen
    // responds to CRUD button clicks
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

                UpdateCommandStates();
            }
        }

        private string _ticketholderErrorMessage = string.Empty;
        public string TicketholderErrorMessage
        {
            get => _ticketholderErrorMessage;
            private set
            {
                if (_ticketholderErrorMessage == value)
                    return;

                _ticketholderErrorMessage = value;
                OnPropertyChanged(nameof(TicketholderErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // combined error message from this and editor
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(TicketholderErrorMessage))
                    return TicketholderErrorMessage;

                return Editor.ErrorMessage;
            }
        }

        public TicketholderEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public event EventHandler<DeleteTicketholderConfirmationEventArguments>? DeleteTicketholderConfirmationRequested;


        public ManageTicketholdersViewModel(TicketholderDAC ticketholderDAC)
        {
            ArgumentNullException.ThrowIfNull(ticketholderDAC);
            _ticketholderDAC = ticketholderDAC;

            Editor = new TicketholderEditorViewModel();

            // listen for editor changes so command states update when validation state changes
            Editor.PropertyChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };

            CreateCommand   
                = new RelayCommand(_ => CreateTicketholder(),   CanCreateTicketholder);
            EditCommand     
                = new RelayCommand(_ => EditTicketholder(),     CanEditTicketholder);
            SaveCommand     
                = new AsyncRelayCommand(_ => SaveTicketholder(),     CanSaveTicketholder);
            DeleteCommand   
                = new AsyncRelayCommand(_ => DeleteTicketholder(),   CanDeleteTicketholder);
            CancelCommand   
                = new RelayCommand(_ => CancelEdit(),           CanCancelEdit);
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
            TicketholderErrorMessage = string.Empty;

            Editor.BeginCreate();

            UpdateCommandStates();
        }

        private void EditTicketholder()
        {
            if (SelectedTicketholder is null)
                return;

            TicketholderErrorMessage = string.Empty;

            Editor.BeginEdit(SelectedTicketholder);

            UpdateCommandStates();
        }

        private async Task SaveTicketholder()
        {
            TicketholderErrorMessage = string.Empty;

            if (!Editor.IsEditing)
                return;

            if (Editor.WorkingCopyTicketholder is null || !Editor.IsValid)
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

            UpdateCommandStates();
        }

        private async Task DeleteTicketholder()
        {
            if (SelectedTicketholder is null)
                return;

            TicketholderErrorMessage = string.Empty;

            var ticketholder = SelectedTicketholder;

            var ticketCount =
                await _ticketholderDAC.GetTicketCountAsync(ticketholder.Id);

            var args = new DeleteTicketholderConfirmationEventArguments(
                ticketholder,
                ticketCount);

            DeleteTicketholderConfirmationRequested?.Invoke(this, args);

            if (!args.Confirmed)
                return;

            await _ticketholderDAC.DeleteAsync(ticketholder.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedTicketholder = null;
        }

        private void CancelEdit()
        {
            TicketholderErrorMessage = string.Empty;

            Editor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreateTicketholder(object? arg)
        {
            return !Editor.IsEditing;
        }

        private bool CanEditTicketholder(object? parameter)
        {
            return SelectedTicketholder is not null;
        }

        private bool CanSaveTicketholder(object? parameter)
        {
            return Editor.WorkingCopyTicketholder is not null &&
                Editor.IsValid;
        }

        private bool CanDeleteTicketholder(object? parameter)
        {
            return SelectedTicketholder is not null;
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
