using SingleStage.Infrastructure.EventArguments;
using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SingleStage.Views
{
    public partial class ManagePerformancesView : UserControl
    {
        public ManagePerformancesView()
        {
            InitializeComponent();
        }

        private async void ManagePerformancesView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged += ManagePerformancesView_DataContextChanged;

            SubscribeToViewModel();

            if (DataContext is ManagePerformancesViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManagePerformancesView: ListOfTicketholders.Count = " +
                        $"{viewModel.ListOfPerformances?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManagePerformancesView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManagePerformancesView: DataContext is not a ManagePerformancesViewModel");
            }
        }

        private void ManagePerformancesView_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManagePerformancesView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManagePerformancesViewModel viewModel)
            {
                viewModel.DeletePerformanceConfirmationRequested +=
                    ManagePerformancesView_DeletePerformanceConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManagePerformancesViewModel viewModel)
            {
                viewModel.DeletePerformanceConfirmationRequested -=
                    ManagePerformancesView_DeletePerformanceConfirmationRequested;
            }
        }

        private void ManagePerformancesView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManagePerformancesViewModel oldViewModel)
            {
                oldViewModel.DeletePerformanceConfirmationRequested -=
                    ManagePerformancesView_DeletePerformanceConfirmationRequested;
            }

            if (e.NewValue is ManagePerformancesViewModel newViewModel)
            {
                newViewModel.DeletePerformanceConfirmationRequested +=
                    ManagePerformancesView_DeletePerformanceConfirmationRequested;
            }
        }

        private void ManagePerformancesView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) 
                return;

            if (DataContext is ManagePerformancesViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }
        private void ManagePerformancesView_DeletePerformanceConfirmationRequested(
            object? sender,
            DeletePerformanceConfirmationEventArguments e)
        {
            string message;

            if (e.ArtistPerformanceCount > 0)
            {
                message =
                    $"The performance '{e.Performance.Description}' has " +
                    $"{e.ArtistPerformanceCount} artist assignment" +
                    $"{(e.ArtistPerformanceCount == 1 ? "" : "s")}.\n\n" +
                    "These artist assignments will also be deleted.\n\n" +
                    "Are you sure you want to delete this performance?";
            }
            else
            {
                message =
                    $"The performance '{e.Performance.Description}' has no artist assignments.\n\n" +
                    "Are you sure you want to delete this performance?";
            }

            var result = MessageBox.Show(
                message,
                "Delete Performance?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }
    }
}