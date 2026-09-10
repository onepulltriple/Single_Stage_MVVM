using Microsoft.Extensions.DependencyInjection;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.Windows;
using System.Windows;
using System.Windows.Input;

namespace SingleStage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider; 

        private readonly ShowDAC _showDAC;

        public CalendarWeekViewModel? CalendarWeekViewModel { get; private set; }

        private CalendarWeekViewModel Calendar =>
            CalendarWeekViewModel ?? throw new InvalidOperationException("The calendar has not been initialized.");

        public DateTime CurrentWeek => Calendar.WeekStart;

        public string WeekDisplayText =>
            $"{CurrentWeek:MMMM d} - {CurrentWeek.AddDays(6):MMMM d}";

        public int SelectedShowTicketCount { get; private set; }

        public string SelectedShowSoldOutText =>
            CalendarWeekViewModel?.SelectedShow?.SoldOut == true
                ? "(SOLD OUT)"
                : string.Empty;


        #region menu commands
        public ICommand ManageShowsCommand { get; }
        
        public ICommand ManagePerformancesCommand { get; }

        public ICommand ManageArtistsCommand { get; }

        public ICommand ManageEmployeesCommand { get; }

        public ICommand ManageTicketholdersCommand { get; }

        public ICommand SellTicketCommand { get; }

        public ICommand ReportsCommand { get; }

        public ICommand ExitCommand { get; }
        #endregion

        #region calendar commands
        public ICommand PreviousWeekCommand { get; }

        public ICommand NextWeekCommand { get; }

        public ICommand TodayCommand { get; }
        #endregion


        // constructor
        public MainWindowViewModel(ShowDAC showDAC, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider 
                ?? throw new ArgumentNullException(nameof(serviceProvider));

            _showDAC = showDAC  
                ?? throw new ArgumentNullException(nameof(showDAC));


            #region menu commands
            ManageShowsCommand =
                new RelayCommand(async _ => await ManageShowsAsync());

            ManagePerformancesCommand =
                new RelayCommand(async _ => await ManagePerformancesAsync());

            ManageArtistsCommand =
                new RelayCommand(ManageArtists);

            ManageEmployeesCommand =
                new RelayCommand(ManageEmployees);

            ManageTicketholdersCommand =
                new RelayCommand(ManageTicketholders);

            SellTicketCommand =
                new RelayCommand(SellTicket);

            ReportsCommand =
                new RelayCommand(Reports);

            ExitCommand =
                new RelayCommand(Exit);

            #endregion

            PreviousWeekCommand =
                new RelayCommand(PreviousWeek);

            NextWeekCommand =
                new RelayCommand(NextWeek);

            TodayCommand =
                new RelayCommand(Today);
        }

        // initialization
        public async Task InitializeAsync()
        {
            List<Show> shows = await _showDAC.GetAllAsync();

            DateTime weekStart = GetMonday(DateTime.Today);

            CalendarWeekViewModel = new CalendarWeekViewModel(
                weekStart,
                shows
                );

            CalendarWeekViewModel.PropertyChanged += CalendarWeekViewModel_PropertyChanged;
            CalendarWeekViewModel.OpenShowRequested += CalendarWeekViewModel_OpenShowRequested;

            OnPropertyChanged(nameof(CalendarWeekViewModel));
            OnPropertyChanged(nameof(CurrentWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }

        private async void CalendarWeekViewModel_PropertyChanged(
            object? sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarWeekViewModel.SelectedShow))
            {
                await UpdateSelectedShowTicketCountAsync();
                OnPropertyChanged(nameof(SelectedShowSoldOutText));
            }
        }

        private async void CalendarWeekViewModel_OpenShowRequested(object? sender, int showId)
        {
            await OpenManageShowsAsync(showId);
        }


        private async Task OpenManageShowsAsync(int showId)
        {
            var window = _serviceProvider.GetRequiredService<ManageShowsWindow>();

            var viewModel = _serviceProvider.GetRequiredService<ManageShowsViewModel>();

            viewModel.InitialShowId = showId;

            window.DataContext = viewModel;

            window.ShowDialog();

            await InitializeAsync();
        }


        private static DateTime GetMonday(DateTime date)
        {
            int day = (int)date.DayOfWeek;

            // convert Sunday (0) to 7
            if (day == 0)
                day = 7;

            return date.Date.AddDays(-(day - 1));
        }


        #region calendar navigation
        private void PreviousWeek()
        {
            Calendar.WeekStart = Calendar.WeekStart.AddDays(-7);

            OnPropertyChanged(nameof(CurrentWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }

        private void NextWeek()
        {
            Calendar.WeekStart = Calendar.WeekStart.AddDays(7);

            OnPropertyChanged(nameof(CurrentWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }

        private void Today()
        {
            Calendar.WeekStart = GetMonday(DateTime.Today);

            OnPropertyChanged(nameof(CurrentWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }
        #endregion

        #region other methods
        private async Task UpdateSelectedShowTicketCountAsync()
        {
            if (Calendar.SelectedShow == null)
            {
                SelectedShowTicketCount = 0;
                OnPropertyChanged(nameof(SelectedShowTicketCount));
                return;
            }

            SelectedShowTicketCount =
                await _showDAC.GetTicketCountAsync(Calendar.SelectedShow.Id);

            OnPropertyChanged(nameof(SelectedShowTicketCount));
        }
        #endregion

        #region menu actions
        private async Task ManageShowsAsync()
        {
            var window = _serviceProvider.GetRequiredService<ManageShowsWindow>();

            // ensure the window's DataContext is the viewModel so InitialiseAsync runs in the view's Loaded handler
            var viewModel = _serviceProvider.GetRequiredService<ManageShowsViewModel>();
            window.DataContext = viewModel;

            // show dialog (blocking)
            window.ShowDialog();
            // when it closes, refresh calendar data

            // reload shows and update calendar
            await InitializeAsync();
        }

        private async Task ManagePerformancesAsync()
        {
            var window = _serviceProvider.GetRequiredService<ManagePerformancesWindow>();
            // viewModel.showId will be null

            // show dialog (blocking)
            window.ShowDialog();

            await InitializeAsync();
        }

        private void ManageArtists()
        {
            var window = _serviceProvider.GetRequiredService<ManageArtistsWindow>();
            window.ShowDialog();
        }

        private void ManageEmployees()
        {
            MessageBox.Show("Manage Employees");
        }

        private void ManageTicketholders()
        {
            var window = _serviceProvider.GetRequiredService<ManageTicketholdersWindow>();
            window.ShowDialog();
        }

        private void SellTicket()
        {
            MessageBox.Show("Sell Ticket");
        }

        private void Reports()
        {
            MessageBox.Show("Reports");
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}