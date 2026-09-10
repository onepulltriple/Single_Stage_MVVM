using System.Collections.ObjectModel;
using SingleStage.DAC;
using SingleStage.DAC.Interfaces;
using SingleStage.Infrastructure.EventArguments;
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
        private readonly IArtistDAC _artistDAC;

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

                UpdateCommandStates();
            }
        }

        public ArtistEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public event EventHandler<DeleteArtistConfirmationEventArguments>? DeleteArtistConfirmationRequested;

        public ManageArtistsViewModel(IArtistDAC artistDAC)
        {
            ArgumentNullException.ThrowIfNull(artistDAC);
            _artistDAC = artistDAC;

            Editor = new ArtistEditorViewModel();

            CreateCommand   = new RelayCommand(_ => CreateArtist(), CanCreateArtist);
            EditCommand     = new RelayCommand(_ => EditArtist(),   CanEditArtist);
            SaveCommand     = new AsyncRelayCommand(_ => SaveArtist(),   CanSaveArtist);
            DeleteCommand   = new AsyncRelayCommand(_ => DeleteArtist(), CanDeleteArtist);
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

            UpdateCommandStates();
        }

        private void EditArtist()
        {
            if (SelectedArtist is null)
                return;

            Editor.BeginEdit(SelectedArtist);

            UpdateCommandStates();
        }

        private async Task SaveArtist()
        {
            if (Editor.WorkingCopyArtist is null)
                return;

            if (string.IsNullOrWhiteSpace(Editor.WorkingCopyArtist.Name))
                return;

            if (Editor.WorkingCopyArtist.Id == 0)
            {
                // new artist
                await _artistDAC.AddAsync(Editor.WorkingCopyArtist);
            }
            else
            {
                // existing artist
                await _artistDAC.UpdateAsync(Editor.WorkingCopyArtist);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedArtist = null;

            UpdateCommandStates();
        }

        private async Task DeleteArtist()
        {
            if (SelectedArtist is null)
                return;

            var artist = SelectedArtist;

            var performanceCount =
                await _artistDAC.GetPerformanceCountAsync(artist.Id);

            var args = new DeleteArtistConfirmationEventArguments(
                artist,
                performanceCount);

            DeleteArtistConfirmationRequested?.Invoke(this, args);

            if (!args.Confirmed)
                return;

            await _artistDAC.DeleteAsync(artist.Id);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedArtist = null;
        }

        private void CancelEdit()
        {
            Editor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreateArtist(object? arg)
        {
            return !Editor.IsEditing;
        }

        private bool CanEditArtist(object? parameter)
        {
            return SelectedArtist is not null;
        }

        private bool CanSaveArtist(object? parameter)
        {
            return Editor.WorkingCopyArtist is not null;
        }

        private bool CanDeleteArtist(object? parameter)
        {
            return SelectedArtist is not null;
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
