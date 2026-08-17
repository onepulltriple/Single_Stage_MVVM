using System.Windows;
using SingleStage.ViewModels;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for ManageTicketholdersWindow.xaml
    /// </summary>
    public partial class ManageTicketholdersWindow : Window
    {
        public ManageTicketholdersWindow(ManageTicketholdersViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel
                ?? throw new ArgumentException(nameof(viewModel));
        }
    }
}
