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
                await viewModel.InitialiseAsync();
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
