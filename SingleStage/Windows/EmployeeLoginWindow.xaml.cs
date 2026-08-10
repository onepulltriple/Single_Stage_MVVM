using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeLoginWindow.xaml
    /// </summary>
    public partial class EmployeeLoginWindow : Window
    {
        private readonly EmployeeDAC _employeeDAC;

        public string? enteredUsername { get; set; }

        public Employee? tempEmployee { get; set; }

        public EmployeeLoginWindow()
        {
            InitializeComponent();
            DataContext = this;

            var context = new SingleStageMvvmContext();
            _employeeDAC = new EmployeeDAC(context);
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(TB00);
        }

        private void TB00KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButtonClicked(sender, e);
        }

        private void PB00KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                Keyboard.Focus(LoginButton);
            if (e.Key == Key.Enter)
                LoginButtonClicked(sender, e);
        }

        private void QuitButtonClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            // check that all fields are filled out
            if (enteredUsername == null ||
                PB00.Password == null)
            {
                UIErrorMessage.Text = "Please fill out all fields.";
                return;
            }

            // check the username exists
            tempEmployee = await _employeeDAC.GetFirstOrDefaultByUsernameAsync(enteredUsername);

            if (tempEmployee == null)
            {
                UIErrorMessage.Text = "Invalid credentials.";
                return;
            }

            // check that entered password matches password in the database
            bool passwordOK = BCrypt.Net.BCrypt.Verify(PB00.Password, tempEmployee.Password);

            if (!passwordOK)
            {
                UIErrorMessage.Text = "Invalid credentials.";
                return;
            }

            // if all checks pass, open main window
            SingleStageMvvmContext context = new();

            ShowDAC showDAC = new(context);
            ArtistDAC artistDAC = new(context);

            MainWindowViewModel vm = new(showDAC, artistDAC);

            await vm.InitializeAsync();

            MainWindow mainWindow = new();
            mainWindow.DataContext = vm;
            mainWindow.Show();

            this.Close();
        }
    }
}
