using SingleStage.Entities;
using System.Net.Mail;
using SingleStage.Infrastructure;

namespace SingleStage.ViewModels.EditorViewModels
{
    // manages/represents the ticketholder currently being edited, i.e. owns the working copy
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

                Validate();
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

                Validate();
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

                Validate();
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

                Validate();
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

        // validation state & message
        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsValid { get; private set; } = false;

        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyTicketholder is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyTicketholder.Name))
            {
                ErrorMessage = "Name is required.";
                RaiseValidityChanged();
                return;
            }

            if (WorkingCopyTicketholder.Birthdate == default)
            {
                ErrorMessage = "Birthdate is required.";
                RaiseValidityChanged();
                return;
            }

            if (WorkingCopyTicketholder.Birthdate > DateTime.Today)
            {
                ErrorMessage = "Birthdate cannot be in the future.";
                RaiseValidityChanged();
                return;
            }

            if (!IsAtLeast18(WorkingCopyTicketholder.Birthdate))
            {
                ErrorMessage = "Ticketholder must be at least 18 years old.";
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyTicketholder.Email))
            {
                ErrorMessage = "Email is required.";
                RaiseValidityChanged();
                return;
            }

            if (!IsValidEmail(WorkingCopyTicketholder.Email))
            {
                ErrorMessage = "Invalid email address.";
                RaiseValidityChanged();
                return;
            }

            IsValid = true;
            RaiseValidityChanged();
        }

        private static bool IsAtLeast18(DateTime birthdate)
        {
            DateTime today = DateTime.Today;

            int age = today.Year - birthdate.Year;

            if (birthdate.Date > today.AddYears(-age))
                age--;

            return age >= 18;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);

                return mailAddress.Address.Equals(
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            WorkingCopyTicketholder = new Ticketholder
            {
                // default to 18 years ago
                Birthdate = DateTime.Today.AddYears(-18),
                Name = string.Empty,
                Email = string.Empty,
                Discount = false
            };
        }

        public void BeginEdit(Ticketholder ticketholder)
        {
            WorkingCopyTicketholder = new Ticketholder
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
            WorkingCopyTicketholder = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}
