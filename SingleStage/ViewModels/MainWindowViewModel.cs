using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using SingleStage.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SingleStage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ShowDAC _showDAC;
        private readonly ArtistDAC _artistDAC;

        //private readonly WindowService _windowService;

        public CalendarWeekViewModel? CalendarWeekViewModel { get; private set; }

        private CalendarWeekViewModel Calendar =>
            CalendarWeekViewModel ?? throw new InvalidOperationException("The calendar has not been initialized.");

        public DateTime CurrentWeek => Calendar.WeekStart;

        public string WeekDisplayText =>
            $"{CurrentWeek:MMMM d} - {CurrentWeek.AddDays(6):MMMM d}";


        // Menu commands
        public ICommand ManageShowsCommand { get; }

        public ICommand ManageArtistsCommand { get; }

        public ICommand ManageEmployeesCommand { get; }

        public ICommand ManageTicketHoldersCommand { get; }

        public ICommand SellTicketCommand { get; }

        public ICommand ReportsCommand { get; }

        public ICommand ExitCommand { get; }


        // Calendar commands
        public ICommand PreviousWeekCommand { get; }

        public ICommand NextWeekCommand { get; }

        public ICommand TodayCommand { get; }


        // Constructor
        public MainWindowViewModel(ShowDAC showDAC, ArtistDAC artistDAC)
        {
            ArgumentNullException.ThrowIfNull(showDAC);
            ArgumentNullException.ThrowIfNull(artistDAC);
            //ArgumentNullException.ThrowIfNull(windowService);
            _showDAC = showDAC;
            _artistDAC = artistDAC;
            //_windowService = windowService;

            ManageShowsCommand =
                new RelayCommand(ManageShows);

            ManageArtistsCommand =
                new RelayCommand(ManageArtists);

            ManageEmployeesCommand =
                new RelayCommand(ManageEmployees);

            ManageTicketHoldersCommand =
                new RelayCommand(ManageTicketholders);

            SellTicketCommand =
                new RelayCommand(SellTicket);

            ReportsCommand =
                new RelayCommand(Reports);

            ExitCommand =
                new RelayCommand(Exit);

            PreviousWeekCommand =
                new RelayCommand(PreviousWeek);

            NextWeekCommand =
                new RelayCommand(NextWeek);

            TodayCommand =
                new RelayCommand(Today);

            ManageArtistsCommand = new RelayCommand(ManageArtists);
        }

        // Initialization
        public async Task InitializeAsync()
        {
            List<Show> shows = await _showDAC.GetAllAsync();

            DateTime weekStart = GetMonday(DateTime.Today);

            CalendarWeekViewModel = new CalendarWeekViewModel(
                weekStart,
                shows
                );

            OnPropertyChanged(nameof(CalendarWeekViewModel));
            OnPropertyChanged(nameof(CurrentWeek));
            OnPropertyChanged(nameof(WeekDisplayText));
        }

        private static DateTime GetMonday(DateTime date)
        {
            int day = (int)date.DayOfWeek;

            // convert Sunday (0) to 7
            if (day == 0)
                day = 7;

            return date.Date.AddDays(-(day - 1));
        }

        // Calendar navigation
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

        // Menu actions
        private void ManageShows()
        {
            MessageBox.Show("Manage Shows");
        }

        private void ManageArtists()
        {
            //_windowService.ShowManageArtists();
            ManageArtistsViewModel manageArtistsViewModel =
                new ManageArtistsViewModel(_artistDAC);

            ManageArtistsWindow window =
                new ManageArtistsWindow
                {
                    DataContext = manageArtistsViewModel
                };

            window.ShowDialog();
        }

        private void ManageEmployees()
        {
            MessageBox.Show("Manage Employees");
        }

        private void ManageTicketholders()
        {
            MessageBox.Show("Manage Ticket Holders");
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
    }
}