using SingleStage.Entities;
using System.Windows;
using System.Windows.Input;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeLoginWindow.xaml
    /// </summary>
    public partial class EmployeeLoginWindow : Window
    {
        readonly SingleStageMvvmContext _context;

        public string? enteredUsername { get; set; }

        public Employee? tempEmployee { get; set; }

        public EmployeeLoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new SingleStageMvvmContext();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(UI00);
        }

        private void TB00KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButtonClicked(sender, e);
        }

        private void PB00KeyDownHandler(object sender, KeyEventArgs e) => TB00KeyDownHandler(sender, e);

        private void QuitButtonClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            // check that all fields are filled out
            if (enteredUsername == null ||
                PB00.Password == null)
            {
                UIErrorMessage.Text ="Please fill out all fields.";
                return;
            }

            // check the username exists
            tempEmployee = _context.Employees.FirstOrDefault(employee => employee.Username == enteredUsername);

            if (tempEmployee == null)
            {
                UIErrorMessage.Text = "Invalid credentials.";
                return;
            }

            // check that entered password matches password in the database
            bool passwordOK = BCrypt.Net.BCrypt.Verify(PB00.Password, tempEmployee.Password);

            if (!passwordOK)
            {
                MessageBox.Show("Invalid credentials.");
                return;
            }

            // if all checks pass, open main dashboard
            MainWindow main = new();
            main.Show();
            this.Close();
        }
    }
}
