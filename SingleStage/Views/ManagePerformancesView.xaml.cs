using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SingleStage.ViewModels;

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
    }
}