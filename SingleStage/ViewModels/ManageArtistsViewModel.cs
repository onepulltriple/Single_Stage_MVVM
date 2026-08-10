using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels.EditorViewModels;


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

                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ArtistEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public ManageArtistsViewModel(ArtistDAC artistDAC)
        {
            ArgumentNullException.ThrowIfNull(artistDAC);
            _artistDAC = artistDAC;

            Editor = new ArtistEditorViewModel();

            CreateCommand   = new RelayCommand(CreateArtist);
            EditCommand     = new RelayCommand(_ => EditArtist(), CanEditArtist);
            SaveCommand     = new RelayCommand(_ => SaveArtist(), CanSaveArtist);
            DeleteCommand   = new RelayCommand(_ => DeleteArtist(), CanDeleteArtist);
        }

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
        }

        private void EditArtist()
        {
            if (SelectedArtist is null)
                return;

            Editor.BeginEdit(SelectedArtist);
        }

        private async void SaveArtist()
        {
            // fill out more later
            await Task.CompletedTask;
        }

        private async void DeleteArtist()
        {
            if (SelectedArtist is null)
                return;

            // fill out more later
            await Task.CompletedTask;
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
    }
}
