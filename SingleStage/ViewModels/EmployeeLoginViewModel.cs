using SingleStage.DAC;
using SingleStage.Infrastructure;
using SingleStage.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace SingleStage.ViewModels
{
    public class EmployeeLoginViewModel : ViewModelBase
    {
        private readonly EmployeeDAC _employeeDAC;

        private readonly IServiceProvider _serviceProvider;

        private string _enteredUsername = string.Empty;
        public string EnteredUsername
        {
            get => _enteredUsername ?? string.Empty;
            set
            {
                if (_enteredUsername == value)
                    return;

                _enteredUsername = value;
                OnPropertyChanged(nameof(EnteredUsername));
            }
        }

        private string _enteredPassword = string.Empty;
        public string EnteredPassword
        {
            get => _enteredPassword ?? string.Empty;
            set
            {
                if (_enteredPassword == value) 
                    return;

                _enteredPassword = value;
                OnPropertyChanged(nameof(EnteredPassword));
            }
        }

        private string _displayedErrorMessage = string.Empty;
        public string DisplayedErrorMessage
        {
            get => _displayedErrorMessage ?? string.Empty;
            set
            {
                if (_displayedErrorMessage == value)
                    return;

                _displayedErrorMessage = value;
                OnPropertyChanged(nameof(DisplayedErrorMessage));
            }
        }

        public AsyncRelayCommand LoginCommand { get; }

        public event EventHandler? LoginSucceeded;

        // constructor receives required services from DI
        public EmployeeLoginViewModel(EmployeeDAC employeeDAC, IServiceProvider serviceProvider)
        {
            _employeeDAC = employeeDAC ?? throw new ArgumentNullException(nameof(employeeDAC));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            LoginCommand = new AsyncRelayCommand(CheckEmployeeAndLogin);
        }

        private async Task CheckEmployeeAndLogin()
        {
            DisplayedErrorMessage = string.Empty;

            // check that all fields are filled out
            if (string.IsNullOrWhiteSpace(EnteredUsername) ||
                string.IsNullOrWhiteSpace(EnteredPassword) )
            {
                DisplayedErrorMessage = "Please fill out all fields.";
                return;
            }

            // check username exists
            Employee? tempEmployee = await _employeeDAC.GetFirstOrDefaultByUsernameAsync(EnteredUsername);

            if (tempEmployee == null)
            {
                DisplayedErrorMessage = "Invalid credentials.";
                return;
            }

            // check that entered password matches password in the database
            bool passwordOK = BCrypt.Net.BCrypt.Verify(EnteredPassword, tempEmployee.Password);

            if (!passwordOK)
            {
                DisplayedErrorMessage = "Invalid credentials.";
                return;
            }

            // if all checks pass, resolve the main window/viewmodel using DI
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            await mainWindowViewModel.InitializeAsync();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();

            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
    }
}
