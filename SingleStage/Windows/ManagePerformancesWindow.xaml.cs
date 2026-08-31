using System.Windows;
using SingleStage.ViewModels;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for ManagePerformancesWindow.xaml
    /// </summary>
    public partial class ManagePerformancesWindow : Window
    {
        public ManagePerformancesWindow(ManagePerformancesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel 
                ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}