using SingleStage.Entities;

// manages/represents the ticketholder currently being edited, i.e. owns the working copy
namespace SingleStage.ViewModels.EditorViewModels
{
    public class TicketholderEditorViewModel : ViewModelBase
    {
        private Ticketholder? _workingCopyTicketholder;
        public Ticketholder? WorkingCopyTicketholder
        {
            get => _workingCopyTicketholder;
            set
            {
                if (_workingCopyTicketholder == value)
                    return;

                _workingCopyTicketholder = value;
                OnPropertyChanged(nameof(WorkingCopyTicketholder));

                // working copy changed, so notify the properties that expose its values, i.e. properties derived from Ticketholder
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Birthdate));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Discount));

                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsEditing => WorkingCopyTicketholder is not null;

        public string Name
        {
            get => WorkingCopyTicketholder?.Name ?? string.Empty;
            set
            {
                if (WorkingCopyTicketholder is null)
                    return;

                if (WorkingCopyTicketholder.Name == value)
                    return;

                WorkingCopyTicketholder.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public DateTime? Birthdate
        {
            get => WorkingCopyTicketholder?.Birthdate;
            set
            {
                if (WorkingCopyTicketholder is null || value is null)
                    return;

                if (WorkingCopyTicketholder.Birthdate == value.Value)
                    return;

                WorkingCopyTicketholder.Birthdate = value.Value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        public string Email
        {
            get => WorkingCopyTicketholder?.Email ?? string.Empty;
            set
            {
                if (WorkingCopyTicketholder is null)
                    return;

                if (WorkingCopyTicketholder.Email == value)
                    return;

                WorkingCopyTicketholder.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public bool Discount
        {
            get => WorkingCopyTicketholder?.Discount ?? false;
            set
            {
                if (WorkingCopyTicketholder is null)
                    return;

                if (WorkingCopyTicketholder.Discount == value)
                    return;

                WorkingCopyTicketholder.Discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }


        public void BeginCreate()
        {
            this.WorkingCopyTicketholder = new Ticketholder
            {
                // initialise sensible default values
                Birthdate = DateTime.Today
            };
        }

        public void BeginEdit(Ticketholder ticketholder)
        {
            this.WorkingCopyTicketholder = new Ticketholder
            {
                Id = ticketholder.Id,
                Name = ticketholder.Name,
                Birthdate = ticketholder.Birthdate,
                Email = ticketholder.Email,
                Discount = ticketholder.Discount
            };
        }

        public void Cancel()
        {
            this.WorkingCopyTicketholder = null;
        }
    }
}
