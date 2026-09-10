using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SingleStage.Infrastructure.EventArguments;

namespace SingleStage.Views
{
    /// <summary>
    /// Interaction logic for ManageArtistsView.xaml
    /// </summary>
    public partial class ManageArtistsView : UserControl
    {
        public ManageArtistsView()
        {
            InitializeComponent();
        }

        private async void ManageArtistsView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged += ManageArtistsView_DataContextChanged;

            SubscribeToViewModel(); 
    
            if (DataContext is ManageArtistsViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageArtistsView: ListOfArtists.Count = " +
                        $"{viewModel.ListOfArtists?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManageArtistsView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManageArtistsView: DataContext is not a ManageArtistsViewModel");
            }
        }

        private void ManageArtistsView_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManageArtistsView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManageArtistsViewModel viewModel)
            {
                viewModel.DeleteArtistConfirmationRequested +=
                    ManageArtistsView_DeleteArtistConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManageArtistsViewModel viewModel)
            {
                viewModel.DeleteArtistConfirmationRequested -=
                    ManageArtistsView_DeleteArtistConfirmationRequested;
            }
        }

        private void ManageArtistsView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManageArtistsViewModel oldViewModel)
            {
                oldViewModel.DeleteArtistConfirmationRequested -=
                    ManageArtistsView_DeleteArtistConfirmationRequested;
            }

            if (e.NewValue is ManageArtistsViewModel newViewModel)
            {
                newViewModel.DeleteArtistConfirmationRequested +=
                    ManageArtistsView_DeleteArtistConfirmationRequested;
            }
        }

        private void ManageArtistsView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (DataContext is ManageArtistsViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void ManageArtistsView_DeleteArtistConfirmationRequested(
            object? sender,
            DeleteArtistConfirmationEventArguments e)
        {
            string message;

            if (e.PerformanceCount > 0)
            {
                message =
                    $"The artist '{e.Artist.Name}' is assigned to " +
                    $"{e.PerformanceCount} performance" +
                    $"{(e.PerformanceCount == 1 ? "" : "s")}.\n\n" +
                    "These performance assignments will also be deleted.\n\n" +
                    "Are you sure you want to delete this artist?";
            }
            else
            {
                message =
                    $"The artist '{e.Artist.Name}' is not assigned to any performances.\n\n" +
                    "Are you sure you want to delete this artist?";
            }

            var result = MessageBox.Show(
                message,
                "Delete Artist?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }
    }
}
