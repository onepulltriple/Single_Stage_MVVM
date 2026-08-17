using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            //if (DataContext is ManageArtistsViewModel viewModel)
            //{
            //    await viewModel.InitialiseAsync();
            //}

            if (DataContext is ManageArtistsViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageArtistsView: ListOfArtists.Count = {viewModel.ListOfArtists?.Count ?? 0}");
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
    }
}
