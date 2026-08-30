using System.Windows;
using SingleStage.ViewModels;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for ManageShowsWindow.xaml
    /// </summary>
    public partial class ManageShowsWindow : Window
    {
        public ManageShowsWindow(ManageShowsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel
                ?? throw new ArgumentException(nameof(viewModel));
        }
    }
}
