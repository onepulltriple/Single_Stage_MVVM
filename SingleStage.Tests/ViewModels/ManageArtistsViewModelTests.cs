using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SingleStage.DAC.Interfaces;
using SingleStage.Entities;
using SingleStage.ViewModels;
using SingleStage.ViewModels.EditorViewModels;

namespace SingleStage.Tests.ViewModels
{
    [TestFixture]
    public class ManageArtistsViewModelTests
    {
        // MethodOrProperty_Condition_ExpectedBehavior
        // arrange
        // act
        // assert

        private IArtistDAC _artistDAC = null!;
        private ManageArtistsViewModel _viewModel = null!;

        [SetUp] // runs before every test
        public void SetUp()
        {
            _artistDAC = Substitute.For<IArtistDAC>();
            _viewModel = new ManageArtistsViewModel(_artistDAC);
        }

        [Test]
        public async Task InitialiseAsync_PopulatesListOfArtists()
        {
            // arrange
            var tempArtists = new List<Artist>
            {
                new Artist {Id = 1, Name = "Prince"},
                new Artist {Id = 2, Name = "The Smiths"},
                new Artist {Id = 3, Name = "Howard Jones"}
            };

            _artistDAC.GetAllAsync().Returns(tempArtists);

            // act
            await _viewModel.InitialiseAsync();

            // assert
            _viewModel.ListOfArtists.Should().HaveCount(3);
            _viewModel.ListOfArtists[0].Name.Should().Be("Prince");
            _viewModel.ListOfArtists[1].Name.Should().Be("The Smiths");
            _viewModel.ListOfArtists[2].Name.Should().Be("Howard Jones");
        }

        [Test]
        public async Task InitialiseAsync_CallsDACGetAllAsync()
        {
            // arrange
            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            // act
            await _viewModel.InitialiseAsync();

            // assert
            await _artistDAC.Received(1).GetAllAsync(); 
            // we expect GetAllAsync() to have been called exactly once
        }

        [Test]
        public async Task InitialiseAsync_ClearsExistingArtists()
        {
            // arrange
            var firstList = new List<Artist>
            {
                new Artist {Id = 1, Name = "Prince"},
                new Artist {Id = 2, Name = "The Smiths"}
            };

            var secondList = new List<Artist>
            {
                new Artist {Id = 3, Name = "New Artist"}
            };

            _artistDAC.GetAllAsync().Returns(firstList, secondList);
            // NSubstitute returns firstList on the first call and secondList on the second call

            await _viewModel.InitialiseAsync(); // first call

            // act
            await _viewModel.InitialiseAsync(); // second call

            // assert
            _viewModel.ListOfArtists.Should().HaveCount(1);
            _viewModel.ListOfArtists[0].Name.Should().Be("New Artist");
        }

        [Test]
        public void CreateCommand_CreatesNewArtist()
        {
            // arrange
            _viewModel.CreateCommand.CanExecute(null).Should().BeTrue();

            // act
            _viewModel.CreateCommand.Execute(null);

            // assert
            _viewModel.Editor.WorkingCopyArtist.Should().NotBeNull();
            _viewModel.Editor.WorkingCopyArtist!.Id.Should().Be(0);
            _viewModel.Editor.IsEditing.Should().BeTrue();
        }

        [Test]
        public void EditCommand_CreatesWorkingCopyOfSelectedArtist()
        {
            // given a selected artist, does Edit() cause the editor to begin editing that artist?
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            _viewModel.SelectedArtist = artist;

            // act
            _viewModel.EditCommand.Execute(null);

            // assert
            _viewModel.Editor.WorkingCopyArtist.Should().NotBeNull();
            _viewModel.Editor.WorkingCopyArtist.Should().NotBeSameAs(artist);
            _viewModel.Editor.WorkingCopyArtist!.Id.Should().Be(15);
            _viewModel.Editor.WorkingCopyArtist!.Name.Should().Be("Sonny Koufax");
        }

        [Test]
        public void EditCommand_WhenArtistIsSelected_CanExecute()
        {
            // arrange
            _viewModel.SelectedArtist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            // assert
            _viewModel.EditCommand.CanExecute(null).Should().BeTrue();
        }


        [Test]
        public void Initially_CommandsHaveExpectedCanExecuteStates()
        {
            // assert
            _viewModel.CreateCommand.CanExecute(null).Should().BeTrue();
            _viewModel.EditCommand.CanExecute(null).Should().BeFalse();
            _viewModel.SaveCommand.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();
            _viewModel.CancelCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void AfterCreate_CommandsHaveExpectedCanExecuteStates()
        {
            // act
            _viewModel.CreateCommand.Execute(null);

            // assert
            _viewModel.CreateCommand.CanExecute(null).Should().BeFalse();
            _viewModel.EditCommand.CanExecute(null).Should().BeFalse();
            _viewModel.SaveCommand.CanExecute(null).Should().BeTrue();
            _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();
            _viewModel.CancelCommand.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public async Task SaveCommand_WhenCreatingNewArtist_AddsArtist()
        {
            // arrange
            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.CreateCommand.Execute(null);
            _viewModel.Editor.Name = "Prince";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            await _artistDAC.Received(1).AddAsync(
                Arg.Is<Artist>(artist =>
                    artist.Id == 0 &&
                    artist.Name == "Prince"));
        }

        [Test]
        public async Task SaveCommand_WhenEditingExistingArtist_UpdatesArtist()
        {
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.SelectedArtist = artist;
            _viewModel.EditCommand.Execute(null);
            _viewModel.Editor.Name = "Kevin Gerrity";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            await _artistDAC.Received(1).UpdateAsync(
                Arg.Is<Artist>(artist =>
                    artist.Id == 15 &&
                    artist.Name == "Kevin Gerrity"));

            await _artistDAC.DidNotReceive().AddAsync(Arg.Any<Artist>());
        }

        [Test]
        public async Task SaveCommand_InitialisesArtistListAfterSaving()
        {
            // arrange
            var savedArtists = new List<Artist>
            {
                new Artist { Id = 1, Name = "Prince" },
                new Artist { Id = 2, Name = "The Smiths" }
            };

            _artistDAC.GetAllAsync().Returns(savedArtists);

            _viewModel.CreateCommand.Execute(null);
            _viewModel.Editor.Name = "Howard Jones";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            _viewModel.ListOfArtists.Should().HaveCount(2);
            _viewModel.ListOfArtists[0].Name.Should().Be("Prince");
            _viewModel.ListOfArtists[1].Name.Should().Be("The Smiths");

            await _artistDAC.Received(1).GetAllAsync();
        }

        [Test]
        public async Task SaveCommand_EndsEditing()
        {
            // arrange
            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.CreateCommand.Execute(null);
            _viewModel.Editor.Name = "Prince";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            _viewModel.Editor.IsEditing.Should().BeFalse();
            _viewModel.Editor.WorkingCopyArtist.Should().BeNull();
        }

        [Test]
        public async Task SaveCommand_ClearsSelectedArtist()
        {
            // arrange
            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.CreateCommand.Execute(null);
            _viewModel.Editor.Name = "Prince";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            _viewModel.SelectedArtist.Should().BeNull();
        }

        [Test]
        public async Task SaveCommand_WhenArtistNameIsEmpty_DoesNotAddArtist()
        {
            // arrange
            _viewModel.CreateCommand.Execute(null);
            _viewModel.Editor.Name = "   ";

            // act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // assert
            await _artistDAC.DidNotReceive().AddAsync(Arg.Any<Artist>());
        }

        [Test]
        public async Task DeleteCommand_DeletesSelectedArtist()
        {
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.SelectedArtist = artist;

            // act
            await _viewModel.DeleteCommand.ExecuteAsync(null);

            // assert
            // when I selected Artist 15 and executed Delete, the DAC must have received DeleteAsync(15)
            await _artistDAC.Received(1).DeleteAsync(15);
        }

        [Test]
        public async Task DeleteCommand_InitialisesArtistListAfterDeleting()
        {
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            var remainingArtists = new List<Artist>
            {
                new Artist { Id = 1, Name = "Prince" },
                new Artist { Id = 2, Name = "The Smiths" }
            };

            _artistDAC.GetAllAsync().Returns(remainingArtists);

            _viewModel.SelectedArtist = artist;

            // act
            await _viewModel.DeleteCommand.ExecuteAsync(null);

            // assert
            _viewModel.ListOfArtists.Should().HaveCount(2);
            _viewModel.ListOfArtists[0].Name.Should().Be("Prince");
            _viewModel.ListOfArtists[1].Name.Should().Be("The Smiths");

            await _artistDAC.Received(1).GetAllAsync();
        }

        [Test]
        public async Task DeleteCommand_ClearsSelectedArtist()
        {
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            _artistDAC.GetAllAsync().Returns(new List<Artist>());

            _viewModel.SelectedArtist = artist;

            // act
            await _viewModel.DeleteCommand.ExecuteAsync(null);

            // assert
            _viewModel.SelectedArtist.Should().BeNull();
        }

        [Test]
        public void CancelCommand_EndsEditing()
        {
            // arrange
            _viewModel.CreateCommand.Execute(null);

            // act
            _viewModel.CancelCommand.Execute(null);

            // assert
            _viewModel.Editor.IsEditing.Should().BeFalse();
            _viewModel.Editor.WorkingCopyArtist.Should().BeNull();
        }

        [Test]
        public void CancelCommand_RestoresExpectedCommandStates()
        {
            // arrange
            _viewModel.CreateCommand.Execute(null);

            // act
            _viewModel.CancelCommand.Execute(null);

            // assert
            _viewModel.CreateCommand.CanExecute(null).Should().BeTrue();
            _viewModel.EditCommand.CanExecute(null).Should().BeFalse();
            _viewModel.SaveCommand.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();
            _viewModel.CancelCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void ChangingSelectedArtist_CancelsCurrentEdit()
        {
            // arrange
            var firstArtist = new Artist
            {
                Id = 1,
                Name = "Prince"
            };

            var secondArtist = new Artist
            {
                Id = 2,
                Name = "The Smiths"
            };

            _viewModel.SelectedArtist = firstArtist;
            _viewModel.EditCommand.Execute(null);

            // act
            _viewModel.SelectedArtist = secondArtist;

            // assert
            _viewModel.Editor.IsEditing.Should().BeFalse();
            _viewModel.Editor.WorkingCopyArtist.Should().BeNull();
        }

        [Test]
        public void ChangingSelectedArtist_UpdatesSelection()
        {
            // arrange
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            // act
            _viewModel.SelectedArtist = artist;

            // assert
            _viewModel.SelectedArtist.Should().BeSameAs(artist);
        }

        [Test]
        public void DeleteCommand_WhenNoArtistIsSelected_CannotExecute()
        {
            // assert
            _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void EditCommand_WhenNoArtistIsSelected_CannotExecute()
        {
            // assert
            _viewModel.EditCommand.CanExecute(null).Should().BeFalse();
        }

    }
}
