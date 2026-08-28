using FluentAssertions;
using NUnit.Framework;
using SingleStage.Entities;
using SingleStage.ViewModels.EditorViewModels;

namespace SingleStage.Tests.ViewModels.EditorViewModels
{
    [TestFixture]
    public class TicketholderEditorViewModelTests
    {
        // MethodOrProperty_Condition_ExpectedBehavior
        // arrange
        // act
        // assert

        [Test]
        public void BeginCreate_CreatesNewTicketholder()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();

            // act
            viewModel.BeginCreate();

            // assert
            viewModel.WorkingCopyTicketholder.Should().NotBeNull();
            viewModel.IsEditing.Should().BeTrue();
            viewModel.Name.Should().BeEmpty();
            viewModel.Birthdate.Should().Be(DateTime.Today);
            viewModel.Email.Should().BeEmpty();
            viewModel.Discount.Should().BeFalse();
        }

        [Test]
        public void BeginEdit_CreatesWorkingCopyHavingPropertiesFromTicketholder()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var ticketholder = new Ticketholder
            {
                Id = 22,
                Name = "Harry Tasker",
                Birthdate = DateTime.Today,
                Email = "harry.tasker@omegasector.gov",
                Discount = false
            };

            // act
            viewModel.BeginEdit(ticketholder);

            // assert
            viewModel.WorkingCopyTicketholder.Should().NotBeNull();
            viewModel.WorkingCopyTicketholder.Should().NotBeSameAs(ticketholder);
            viewModel.WorkingCopyTicketholder!.Id.Should().Be(22);
            viewModel.WorkingCopyTicketholder!.Name.Should().Be("Harry Tasker");
            viewModel.WorkingCopyTicketholder!.Birthdate.Should().Be(DateTime.Today);
            viewModel.WorkingCopyTicketholder!.Email.Should().Be("harry.tasker@omegasector.gov");
            viewModel.WorkingCopyTicketholder!.Discount.Should().BeFalse();
            viewModel.IsEditing.Should().BeTrue();
        }

        [Test]
        public void BeginEdit_SetViewModelPropertiesToThoseFromTicketholder()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var ticketholder = new Ticketholder
            {
                Id = 22,
                Name = "Harry Tasker",
                Birthdate = DateTime.Today,
                Email = "harry.tasker@omegasector.gov",
                Discount = false
            };

            // act
            viewModel.BeginEdit(ticketholder);

            // assert
            viewModel.Name.Should().Be("Harry Tasker");
            viewModel.Birthdate.Should().Be(DateTime.Today);
            viewModel.Email.Should().Be("harry.tasker@omegasector.gov");
            viewModel.Discount.Should().BeFalse();
            viewModel.IsEditing.Should().BeTrue();
        }

        [Test]
        public void BeginEdit_DoesNotModifyOriginalTicketholder()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var ticketholder = new Ticketholder
            {
                Id = 22,
                Name = "Harry Tasker",
                Birthdate = DateTime.Today,
                Email = "harry.tasker@omegasector.gov",
                Discount = false
            };

            // act
            viewModel.BeginEdit(ticketholder);
            viewModel.Name = "Albert Gibson";

            // assert
            ticketholder.Name.Should().Be("Harry Tasker");
            viewModel.Name.Should().Be("Albert Gibson");
        }

        [Test]
        public void Name_Get_ReturnsWorkingCopyName()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();
            viewModel.WorkingCopyTicketholder!.Name = "Harry Tasker";

            // act
            var result = viewModel.Name;

            // assert
            result.Should().Be("Harry Tasker");
        }

        [Test]
        public void Name_Set_UpdatesWorkingCopyName()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Name = "Albert Gibson";

            // assert
            viewModel.WorkingCopyTicketholder!.Name.Should().Be("Albert Gibson");
            viewModel.Name.Should().Be("Albert Gibson");
        }

        [Test]
        public void Name_Set_WhenNoTicketholderIsBeingEdited_DoesNothing()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();

            // act
            // do not call BeginCreate();
            viewModel.Name = "Albert Gibson";

            // assert
            viewModel.WorkingCopyTicketholder.Should().BeNull();
            viewModel.Name.Should().BeEmpty();
            viewModel.IsEditing.Should().BeFalse();
        }

        [Test]
        public void Birthdate_Get_ReturnsWorkingCopyBirthdate()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            var birthdate = new DateTime(1985, 6, 15);
            viewModel.WorkingCopyTicketholder!.Birthdate = birthdate;

            // act
            var result = viewModel.Birthdate;

            // assert
            result.Should().Be(birthdate);
        }

        [Test]
        public void Birthdate_Set_UpdatesWorkingCopyBirthdate()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            var birthdate = new DateTime(1985, 6, 15);

            // act
            viewModel.Birthdate = birthdate;

            // assert
            viewModel.WorkingCopyTicketholder!.Birthdate.Should().Be(birthdate);
            viewModel.Birthdate.Should().Be(birthdate);
        }

        [Test]
        public void Birthdate_Set_WhenNoTicketholderIsBeingEdited_DoesNothing()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var birthdate = new DateTime(1985, 6, 15);

            // act
            // do not call BeginCreate();
            viewModel.Birthdate = birthdate;

            // assert
            viewModel.WorkingCopyTicketholder.Should().BeNull();
            viewModel.Birthdate.Should().BeNull();
            viewModel.IsEditing.Should().BeFalse();
        }

        [Test]
        public void Email_Get_ReturnsWorkingCopyEmail()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();
            viewModel.WorkingCopyTicketholder!.Email = "john.smith@example.com";

            // act
            var result = viewModel.Email;

            // assert
            result.Should().Be("john.smith@example.com");
        }

        [Test]
        public void Email_Set_UpdatesWorkingCopyEmail()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Email = "john.smith@example.com";

            // assert
            viewModel.WorkingCopyTicketholder!.Email.Should().Be("john.smith@example.com");
            viewModel.Email.Should().Be("john.smith@example.com");
        }

        [Test]
        public void Email_Set_WhenNoTicketholderIsBeingEdited_DoesNothing()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();

            // act
            // do not call BeginCreate();
            viewModel.Email = "john.smith@example.com";

            // assert
            viewModel.WorkingCopyTicketholder.Should().BeNull();
            viewModel.Email.Should().BeEmpty();
            viewModel.IsEditing.Should().BeFalse();
        }

        [Test]
        public void Discount_Get_ReturnsWorkingCopyDiscount()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();
            viewModel.WorkingCopyTicketholder!.Discount = true;

            // act
            var result = viewModel.Discount;

            // assert
            result.Should().BeTrue();
        }

        [Test]
        public void Discount_Set_UpdatesWorkingCopyDiscount()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Discount = true;

            // assert
            viewModel.WorkingCopyTicketholder!.Discount.Should().BeTrue();
            viewModel.Discount.Should().BeTrue();
        }

        [Test]
        public void Discount_Set_WhenNoTicketholderIsBeingEdited_DoesNothing()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();

            // act
            // do not call BeginCreate();
            viewModel.Discount = true;

            // assert
            viewModel.WorkingCopyTicketholder.Should().BeNull();
            viewModel.Discount.Should().BeFalse();
            viewModel.IsEditing.Should().BeFalse();
        }


        [Test]
        public void Cancel_ClearsWorkingCopy()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Cancel();

            // assert
            viewModel.WorkingCopyTicketholder.Should().BeNull();
        }

        [Test]
        public void Cancel_EndsEditing()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            // act
            viewModel.Cancel();

            // assert
            viewModel.IsEditing.Should().BeFalse();
            viewModel.Name.Should().BeEmpty();
            viewModel.Birthdate.Should().BeNull();
            viewModel.Email.Should().BeEmpty();
            viewModel.Discount.Should().BeFalse();
        }

        // to test INotifyPropertyChanged events:
        // - subscribe to PropertyChanged in the test
        // - perform the action
        // - inspect which property name(s) was(were) reported

        [Test]
        public void BeginCreate_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName);
            };

            // act
            viewModel.BeginCreate();

            // assert
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.WorkingCopyTicketholder));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Name));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Birthdate));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Email));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Discount));
        }

        [Test]
        public void BeginEdit_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            var ticketholder = new Ticketholder
            {
                Id = 22,
                Name = "Harry Tasker",
                Birthdate = DateTime.Today,
                Email = "harry.tasker@omegasector.gov",
                Discount = false
            };

            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName);
            };

            // act
            viewModel.BeginEdit(ticketholder);

            // assert
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.WorkingCopyTicketholder));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Name));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Birthdate));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Email));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Discount));
        }

        [Test]
        public void Cancel_RaisesExpectedPropertyChangedNotifications()
        {
            // arrange
            var viewModel = new TicketholderEditorViewModel();
            viewModel.BeginCreate();

            var changedProperties = new List<string?>();

            viewModel.PropertyChanged += (_, args) =>
            {
                changedProperties.Add(args.PropertyName);
            };

            // act
            viewModel.Cancel();

            // assert
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.WorkingCopyTicketholder));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.IsEditing));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Name));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Birthdate));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Email));
            changedProperties.Should().Contain(nameof(TicketholderEditorViewModel.Discount));
        }
    }
}
