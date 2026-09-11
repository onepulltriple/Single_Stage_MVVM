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

        public DateTime CalendarStartDate => Calendar.CalendarStartDate;

        public DateTime CurrentWeekMonday => GetMonday(CalendarStartDate);


        public int CalendarWeek =>
            System.Globalization.ISOWeek.GetWeekOfYear(CurrentWeekMonday);

        public string WeekDisplayText =>
            $"Week {CalendarWeek}:  {CurrentWeekMonday:MMMM d} - {CurrentWeekMonday.AddDays(6):MMMM d}, {CurrentWeekMonday:yyyy}";

        public int SelectedShowTicketCount { get; private set; }

        public string SelectedShowSoldOutText =>
            CalendarWeekViewModel?.SelectedShow?.SoldOut == true
                ? "(SOLD OUT)"
                : string.Empty;

        private DateTime? _jumpToDate = DateTime.Today;
        public DateTime? JumpToDate
        {
            get => _jumpToDate;

            set
            {
                DateTime? newDate = value?.Date;

                if (_jumpToDate == newDate)
                    return;

                _jumpToDate = newDate;

                OnPropertyChanged(nameof(JumpToDate));

                if (newDate.HasValue && CalendarWeekViewModel is not null)
                {
                    Calendar.CalendarStartDate = GetMonday(newDate.Value);

                    NotifyCalendarNavigationProperties();
                }
            }
        }




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

        public ICommand PreviousDayCommand { get; }

        public ICommand NextDayCommand { get; }

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

            PreviousDayCommand =
                new RelayCommand(PreviousDay);

            NextDayCommand =
                new RelayCommand(NextDay);
        }

        // initialization
        public async Task InitializeAsync()
        {
            List<Show> shows = await _showDAC.GetAllAsync();

            DateTime calendarStartDate;
            DateTime? jumpToDate;

            if (CalendarWeekViewModel is null)
            {
                calendarStartDate = GetMonday(DateTime.Today);
                jumpToDate = DateTime.Today;
            }
            else
            {
                calendarStartDate = Calendar.CalendarStartDate.Date;
                jumpToDate = JumpToDate;

                if (!jumpToDate.HasValue ||
                    jumpToDate.Value < calendarStartDate ||
                    jumpToDate.Value > calendarStartDate.AddDays(6))
                {
                    jumpToDate = calendarStartDate;
                }
            }

            CalendarWeekViewModel = new CalendarWeekViewModel(
                calendarStartDate,
                shows
            );


            CalendarWeekViewModel.PropertyChanged += CalendarWeekViewModel_PropertyChanged;
            CalendarWeekViewModel.OpenShowRequested += CalendarWeekViewModel_OpenShowRequested;

            _jumpToDate = jumpToDate?.Date;

            OnPropertyChanged(nameof(CalendarWeekViewModel));
            OnPropertyChanged(nameof(JumpToDate));
            NotifyCalendarNavigationProperties();
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


        #region calendar navigation
        private static DateTime GetMonday(DateTime date)
        {
            int day = (int)date.DayOfWeek;

            // convert Sunday (0) to 7
            if (day == 0)
                day = 7;

            return date.Date.AddDays(-(day - 1));
        }

        private void PreviousWeek()
        {
            DateTime? currentJumpToDate = JumpToDate;

            Calendar.CalendarStartDate =
                Calendar.CalendarStartDate.AddDays(-7);

            UpdateJumpToDateWithoutNavigation(
                currentJumpToDate?.AddDays(-7)
                ?? Calendar.CalendarStartDate);

            NotifyCalendarNavigationProperties();
        }

        private void NextWeek()
        {
            DateTime? currentJumpToDate = JumpToDate;

            Calendar.CalendarStartDate =
                Calendar.CalendarStartDate.AddDays(7);

            UpdateJumpToDateWithoutNavigation(
                currentJumpToDate?.AddDays(7)
                ?? Calendar.CalendarStartDate);

            NotifyCalendarNavigationProperties();
        }

        private void Today()
        {
            DateTime today = DateTime.Today;

            Calendar.CalendarStartDate = GetMonday(today);

            UpdateJumpToDateWithoutNavigation(today);

            NotifyCalendarNavigationProperties();
        }

        private void PreviousDay()
        {
            DateTime? currentJumpToDate = JumpToDate;

            Calendar.CalendarStartDate =
                Calendar.CalendarStartDate.AddDays(-1);

            UpdateJumpToDateWithoutNavigation(
                currentJumpToDate?.AddDays(-1)
                ?? Calendar.CalendarStartDate);

            NotifyCalendarNavigationProperties();
        }

        private void NextDay()
        {
            DateTime? currentJumpToDate = JumpToDate;

            Calendar.CalendarStartDate =
                Calendar.CalendarStartDate.AddDays(1);

            UpdateJumpToDateWithoutNavigation(
                currentJumpToDate?.AddDays(1)
                ?? Calendar.CalendarStartDate);

            NotifyCalendarNavigationProperties();
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

        private void NotifyCalendarNavigationProperties()
        {
            OnPropertyChanged(nameof(CalendarStartDate));
            OnPropertyChanged(nameof(CurrentWeekMonday));
            OnPropertyChanged(nameof(CalendarWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }

        private void UpdateJumpToDateWithoutNavigation(DateTime? date)
        {
            _jumpToDate = date?.Date;

            OnPropertyChanged(nameof(JumpToDate));
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