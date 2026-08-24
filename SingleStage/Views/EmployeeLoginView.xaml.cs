using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SingleStage.Views
{
    /// <summary>
    /// Interaction logic for EmployeeLoginView.xaml
    /// </summary>
    public partial class EmployeeLoginView : UserControl
    {
        public EmployeeLoginView()
        {
            InitializeComponent();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(TB00);
        }

        private void TB00KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
                e.Handled = true;
            }
        }

        private void PB00KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                Keyboard.Focus(LoginButton);
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                Login();
                e.Handled = true;
            }
        }

        private void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (DataContext is EmployeeLoginViewModel viewModel)
            {
                viewModel.LoginCommand.Execute(null);
            }
        }

        private void QuitButtonClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
