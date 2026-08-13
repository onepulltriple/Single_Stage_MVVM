using SingleStage.Entities;

// manages/represents the artist currently being edited, i.e. owns the working copy
namespace SingleStage.ViewModels.EditorViewModels
{
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
                OnPropertyChanged(nameof(IsEditing));
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
            }
        }

        public void BeginCreate()
        {
            this.WorkingCopyArtist = new Artist();
        }

        public void BeginEdit(Artist artist)
        {
            this.WorkingCopyArtist = new Artist
            {
                Id = artist.Id,
                Name = artist.Name,
            };
        }

        public void Cancel()
        {
            this.WorkingCopyArtist = null;
        }
    }
}
