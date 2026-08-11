using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
    }
}
