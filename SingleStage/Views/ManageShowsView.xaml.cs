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
                await viewModel.InitialiseAsync();
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