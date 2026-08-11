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
            if (DataContext is ManageArtistsViewModel viewModel)
            {
                await viewModel.InitialiseAsync();
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
