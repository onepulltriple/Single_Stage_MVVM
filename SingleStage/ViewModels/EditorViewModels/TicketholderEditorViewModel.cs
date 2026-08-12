using SingleStage.Entities;

// manages/represents the ticketholder currently being edited, i.e. owns the working copy
namespace SingleStage.ViewModels.EditorViewModels
{
    public class TicketholderEditorViewModel : ViewModelBase
    {
        private Ticketholder? _ticketholder;
        public Ticketholder? Ticketholder
        {
            get => _ticketholder;
            set
            {
                if (_ticketholder == value)
                    return;

                _ticketholder = value;
                OnPropertyChanged(nameof(Ticketholder));
            }
        }

        public bool IsEditing => Ticketholder is not null;

        public void BeginCreate()
        {
            this.Ticketholder = new Ticketholder();

            OnPropertyChanged(nameof(IsEditing));
        }

        public void BeginEdit(Ticketholder ticketholder)
        {
            this.Ticketholder = new Ticketholder
            {
                Id = ticketholder.Id,
                Name = ticketholder.Name,
                Birthdate = ticketholder.Birthdate,
                Email = ticketholder.Email,
                Discount = ticketholder.Discount
            };

            OnPropertyChanged(nameof(IsEditing));
        }

        public void Cancel()
        {
            this.Ticketholder = null;
            OnPropertyChanged(nameof(IsEditing));
        }
    }
}
