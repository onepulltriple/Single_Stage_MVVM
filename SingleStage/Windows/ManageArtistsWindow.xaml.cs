using System.Windows;
using SingleStage.ViewModels;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for ManageArtistsWindow.xaml
    /// </summary>
    public partial class ManageArtistsWindow : Window
    {
        public ManageArtistsWindow(ManageArtistsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
