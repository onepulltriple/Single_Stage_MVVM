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
            if (DataContext is ManageTicketholdersViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageTicketholdersView: ListOfTicketholders.Count = {viewModel.ListOfTicketholders?.Count ?? 0}");
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
    }
}
