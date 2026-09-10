using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SingleStage.ViewModels;
using SingleStage.Infrastructure.EventArguments;


namespace SingleStage.Views
{
    /// <summary>
    /// Interaction logic for ManageShowsView.xaml
    /// </summary>
    public partial class ManageShowsView : UserControl
    {
        public ManageShowsView()
        {
            InitializeComponent();
        }

        private async void ManageShowsView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged += ManageShowsView_DataContextChanged;

            SubscribeToViewModel();

            if (DataContext is ManageShowsViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageShowsView: ListOfTicketholders.Count = " +
                        $"{viewModel.ListOfShows?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManageShowsView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManageShowsView: DataContext is not a ManageShowsViewModel");
            }
        }

        private void ManageShowsView_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManageShowsView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManageShowsViewModel viewModel)
            {
                viewModel.DeleteShowConfirmationRequested +=
                    ManageShowsView_DeleteShowConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManageShowsViewModel viewModel)
            {
                viewModel.DeleteShowConfirmationRequested -=
                    ManageShowsView_DeleteShowConfirmationRequested;
            }
        }

        private void ManageShowsView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManageShowsViewModel oldViewModel)
            {
                oldViewModel.DeleteShowConfirmationRequested -=
                    ManageShowsView_DeleteShowConfirmationRequested;
            }

            if (e.NewValue is ManageShowsViewModel newViewModel)
            {
                newViewModel.DeleteShowConfirmationRequested +=
                    ManageShowsView_DeleteShowConfirmationRequested;
            }
        }

        private void ManageShowsView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (DataContext is ManageShowsViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void ManageShowsView_DeleteShowConfirmationRequested(
            object? sender,
            DeleteShowConfirmationEventArguments e)
        {
            string message;

            if (e.PerformanceCount > 0)
            {
                message =
                    $"The show '{e.Show.Name}' has " +
                    $"{e.PerformanceCount} performance" +
                    $"{(e.PerformanceCount == 1 ? "" : "s")} associated with it.\n\n" +
                    "These performances will also be deleted.\n\n" +
                    "Are you sure you want to delete this show?";
            }
            else
            {
                message =
                    $"The show '{e.Show.Name}' has no performances associated with it.\n\n" +
                    "Are you sure you want to delete this show?";
            }

            var result = MessageBox.Show(
                message,
                "Delete Show?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }
    }
}