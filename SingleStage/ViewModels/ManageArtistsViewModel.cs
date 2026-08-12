using System.Collections.ObjectModel;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;

// manages the artist-management screen as a whole
// owns the list of artists
// owns the currently selected row
// owns the commands for the screen
// responds to CRUD button clicks
namespace SingleStage.ViewModels
{
    public class ManageArtistsViewModel : ViewModelBase
    {
        private readonly ArtistDAC _artistDAC;

        public ObservableCollection<Artist> ListOfArtists { get; } = new();

        private Artist? _selectedArtist;
        public Artist? SelectedArtist
        {
            get => _selectedArtist;
            set
            {
                if (_selectedArtist == value)
                    return;

                _selectedArtist = value;
                OnPropertyChanged(nameof(SelectedArtist));

                // changing the selected artist cancels any changes currently being made
                Editor.Cancel();

                EditCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
            }
        }

        public ArtistEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ManageArtistsViewModel(ArtistDAC artistDAC)
        {
            ArgumentNullException.ThrowIfNull(artistDAC);
            _artistDAC = artistDAC;

            Editor = new ArtistEditorViewModel();

            CreateCommand   = new RelayCommand(CreateArtist);
            EditCommand     = new RelayCommand(_ => EditArtist(),   CanEditArtist);
            SaveCommand     = new RelayCommand(_ => SaveArtist(),   CanSaveArtist);
            DeleteCommand   = new RelayCommand(_ => DeleteArtist(), CanDeleteArtist);
            CancelCommand   = new RelayCommand(_ => CancelEdit(),   CanCancelEdit);
        }

        // loads the list
        public async Task InitialiseAsync()
        {
            var artists = await _artistDAC.GetAllAsync();

            ListOfArtists.Clear();

            foreach (Artist artist in artists)
            {
                ListOfArtists.Add(artist);
            }
        }

        private void CreateArtist()
        {
            Editor.BeginCreate();

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private void EditArtist()
        {
            if (SelectedArtist is null)
                return;

            Editor.BeginEdit(SelectedArtist);

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private async void SaveArtist()
        {
            if (Editor.Artist is null)
                return;

            if (string.IsNullOrWhiteSpace(Editor.Artist.Name))
                return;

            if (Editor.Artist.Id == 0)
            {
                // new artist
                await _artistDAC.AddAsync(Editor.Artist);
            }
            else
            {
                // existing artist
                await _artistDAC.UpdateAsync(Editor.Artist);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedArtist = null;
        }

        private async void DeleteArtist()
        {
            if (SelectedArtist is null)
                return;

            await _artistDAC.DeleteAsync(SelectedArtist.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedArtist = null;
        }

        private void CancelEdit()
        {
            Editor.Cancel();

            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private bool CanEditArtist(object? parameter)
        {
            return SelectedArtist is not null;
        }

        private bool CanSaveArtist(object? parameter)
        {
            return Editor.Artist is not null;
        }

        private bool CanDeleteArtist(object? parameter)
        {
            return SelectedArtist is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return Editor.IsEditing;
        }
    }
}
