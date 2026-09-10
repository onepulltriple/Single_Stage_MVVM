using SingleStage.Infrastructure.EventArguments;
using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SingleStage.Views
{
    /// <summary>
    /// Interaction logic for ManageTicketholdersView.xaml
    /// </summary>
    public partial class ManageTicketholdersView : UserControl
    {
        public ManageTicketholdersView()
        {
            InitializeComponent();
        }

        private async void ManageTicketholdersView_Loaded(object sender, RoutedEventArgs e)
        {

            DataContextChanged += ManageTicketholdersView_DataContextChanged;

            SubscribeToViewModel();

            if (DataContext is ManageTicketholdersViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageTicketholdersView: ListOfTicketholders.Count = " +
                        $"{viewModel.ListOfTicketholders?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManageTicketholdersView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManageTicketholdersView: DataContext is not a ManageTicketholdersViewModel");
            }
        }

        private void ManageTicketholdersView_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManageTicketholdersView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManageTicketholdersViewModel viewModel)
            {
                viewModel.DeleteTicketholderConfirmationRequested +=
                    ManageTicketholdersView_DeleteTicketholderConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManageTicketholdersViewModel viewModel)
            {
                viewModel.DeleteTicketholderConfirmationRequested -=
                    ManageTicketholdersView_DeleteTicketholderConfirmationRequested;
            }
        }

        private void ManageTicketholdersView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManageTicketholdersViewModel oldViewModel)
            {
                oldViewModel.DeleteTicketholderConfirmationRequested -=
                    ManageTicketholdersView_DeleteTicketholderConfirmationRequested;
            }

            if (e.NewValue is ManageTicketholdersViewModel newViewModel)
            {
                newViewModel.DeleteTicketholderConfirmationRequested +=
                    ManageTicketholdersView_DeleteTicketholderConfirmationRequested;
            }
        }


        private void ManageTicketholdersView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (DataContext is ManageTicketholdersViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void ManageTicketholdersView_DeleteTicketholderConfirmationRequested(
            object? sender,
            DeleteTicketholderConfirmationEventArguments e)
        {
            string message;

            if (e.TicketCount > 0)
            {
                message =
                    $"The ticketholder '{e.Ticketholder.Name}' has " +
                    $"{e.TicketCount} ticket" +
                    $"{(e.TicketCount == 1 ? "" : "s")}.\n\n" +
                    "These tickets will also be deleted.\n\n" +
                    "Are you sure you want to delete this ticketholder?";
            }
            else
            {
                message =
                    $"The ticketholder '{e.Ticketholder.Name}' has no tickets.\n\n" +
                    "Are you sure you want to delete this ticketholder?";
            }

            var result = MessageBox.Show(
                message,
                "Delete Ticketholder?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }

    }
}
