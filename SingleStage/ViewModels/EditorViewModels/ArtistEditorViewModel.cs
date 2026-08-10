using SingleStage.Entities;
using SingleStage.Infrastructure;

namespace SingleStage.ViewModels.EditorViewModels
{
    public class ArtistEditorViewModel : ViewModelBase
    {
        // the editor owns the working copy

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
            Artist = new Artist();

            OnPropertyChanged(nameof(IsEditing));
        }

        public void BeginEdit(Artist artist)
        {
            Artist = new Artist
            {
                Id = artist.Id,
                Name = artist.Name,
            };

            OnPropertyChanged(nameof(IsEditing));
        }

        public void Cancel()
        {
            Artist = null;
            OnPropertyChanged(nameof(IsEditing));
        }
    }
}
