using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SingleStage.ViewModels;

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
    }
}