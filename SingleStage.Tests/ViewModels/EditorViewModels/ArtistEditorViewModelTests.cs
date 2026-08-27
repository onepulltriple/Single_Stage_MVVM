using FluentAssertions;
using NUnit.Framework;
using SingleStage.Entities;
using SingleStage.ViewModels.EditorViewModels;

namespace SingleStage.Tests.ViewModels.EditorViewModels
{
    [TestFixture]
    public class ArtistEditorViewModelTests
    {
        // MethodOrProperty_Condition_ExpectedBehavior
        // arrange
        // act
        // assert

        [Test]
        public void BeginCreate_CreatesNewArtist()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();

            // act
            viewModel.BeginCreate();

            // assert
            viewModel.WorkingCopyArtist.Should().NotBeNull();
            viewModel.IsEditing.Should().BeTrue();
            viewModel.Name.Should().BeEmpty();
        }

        [Test]
        public void BeginEdit_CreatesWorkingCopyHavingPropertiesFromArtist()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            // act
            viewModel.BeginEdit(artist);

            // assert
            viewModel.WorkingCopyArtist.Should().NotBeNull();
            viewModel.WorkingCopyArtist.Should().NotBeSameAs(artist);
            viewModel.WorkingCopyArtist!.Id.Should().Be(15);
            viewModel.WorkingCopyArtist.Name.Should().Be("Sonny Koufax");
            viewModel.IsEditing.Should().BeTrue();
        }

        [Test]
        public void BeginEdit_SetViewModelPropertiesToThoseFromArtist()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            // act
            viewModel.BeginEdit(artist);

            // assert
            viewModel.Name.Should().Be("Sonny Koufax");
        }

        [Test]
        public void BeginEdit_DoesNotModifyOriginalArtist()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            // act
            viewModel.BeginEdit(artist);
            viewModel.Name = "Kevin Gerrity";

            // assert
            artist.Name.Should().Be("Sonny Koufax");
            viewModel.Name.Should().Be("Kevin Gerrity");
        }

        [Test]
        public void Name_Get_ReturnsWorkingCopyName()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            viewModel.BeginCreate();
            viewModel.WorkingCopyArtist!.Name = "Sonny Koufax";

            // act
            var result = viewModel.Name;

            // assert
            result.Should().Be("Sonny Koufax");
        }

        [Test]
        public void Name_Set_UpdatesWorkingCopyName()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Name = "John Smith";

            // assert
            viewModel.WorkingCopyArtist!.Name.Should().Be("John Smith");
            viewModel.Name.Should().Be("John Smith");
        }

        [Test]
        public void Name_Set_WhenNoArtistIsBeingEdited_DoesNothing()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();

            // act
            viewModel.Name = "John Smith";

            // assert
            viewModel.WorkingCopyArtist.Should().BeNull();
            viewModel.Name.Should().BeEmpty();
        }

        [Test]
        public void Cancel_ClearsWorkingCopy()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Cancel();

            // assert
            viewModel.WorkingCopyArtist.Should().BeNull();
        }

        [Test]
        public void Cancel_EndsEditing()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Cancel();

            // assert
            viewModel.IsEditing.Should().BeFalse();
            viewModel.Name.Should().BeEmpty();
        }

        // to test INotifyPropertyChanged events:
        // - subscribe to PropertyChanged in the test
        // - perform the action
        // - inspect which property name(s) was(were) reported

        [Test]
        public void BeginCreate_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName); 
            };

            // act
            viewModel.BeginCreate();

            // assert
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.WorkingCopyArtist));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.Name));
        }

        [Test]
        public void BeginEdit_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            var artist = new Artist
            {
                Id = 15,
                Name = "Sonny Koufax"
            };

            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName);
            };

            // act
            viewModel.BeginEdit(artist);

            // assert
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.WorkingCopyArtist));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.Name));
        }

        [Test]
        public void Cancel_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new ArtistEditorViewModel();
            viewModel.BeginCreate();

            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName);
            };

            // act
            viewModel.Cancel();

            // assert
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.WorkingCopyArtist));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(ArtistEditorViewModel.Name));
        }
    }
}
