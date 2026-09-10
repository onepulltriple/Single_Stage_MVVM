using SingleStage.Entities;

namespace SingleStage.ViewModels.EditorViewModels
{
    // manages/represents the artist currently being edited, i.e. owns the working copy
    public class ArtistEditorViewModel : ViewModelBase
    {
        private Artist? _workingCopyArtist;
        public Artist? WorkingCopyArtist
        {
            get => _workingCopyArtist;
            set
            {
                if (_workingCopyArtist == value)
                    return;

                _workingCopyArtist = value;

                OnPropertyChanged(nameof(WorkingCopyArtist));

                // working copy changed, so notify the properties that expose its values, i.e. properties derived from Artist
                OnPropertyChanged(nameof(Name));

                OnPropertyChanged(nameof(IsEditing));

                Validate();
            }
        }

        public bool IsEditing => WorkingCopyArtist is not null;

        public string Name
        {
            get => WorkingCopyArtist?.Name ?? string.Empty;
            set
            {
                if (WorkingCopyArtist is null)
                    return;

                if (WorkingCopyArtist.Name == value)
                    return;

                WorkingCopyArtist.Name = value;

                OnPropertyChanged(nameof(Name));

                Validate();
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

            if (WorkingCopyArtist is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyArtist.Name))
            {
                ErrorMessage = "Name is required.";
                RaiseValidityChanged();
                return;
            }

            IsValid = true;
            RaiseValidityChanged();
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            WorkingCopyArtist = new Artist
            {
                Name = string.Empty
            };
        }

        // shallow clone to avoid editing the original instance directly
        public void BeginEdit(Artist artist)
        {
            WorkingCopyArtist = new Artist
            {
                Id = artist.Id,
                Name = artist.Name,
            };
        }

        public void Cancel()
        {
            WorkingCopyArtist = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}
