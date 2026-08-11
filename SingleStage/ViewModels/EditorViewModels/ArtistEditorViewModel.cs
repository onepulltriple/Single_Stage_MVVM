using SingleStage.Entities;
using SingleStage.Infrastructure;

// manages/represents the artist currently being edited, i.e. owns the working copy
namespace SingleStage.ViewModels.EditorViewModels
{
    public class ArtistEditorViewModel : ViewModelBase
    {
        private Artist? _artist;

        public Artist? Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public bool IsEditing => Artist is not null;

        public void BeginCreate()
        {
            this.Artist = new Artist();

            OnPropertyChanged(nameof(IsEditing));
        }

        public void BeginEdit(Artist artist)
        {
            this.Artist = new Artist
            {
                Id = artist.Id,
                Name = artist.Name,
            };

            OnPropertyChanged(nameof(IsEditing));
        }

        public void Cancel()
        {
            this.Artist = null;
            OnPropertyChanged(nameof(IsEditing));
        }
    }
}
