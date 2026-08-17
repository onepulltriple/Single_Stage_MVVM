using Microsoft.Extensions.DependencyInjection;
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

        private readonly IServiceProvider _serviceProvider; // used to resolve windows on demand


        public CalendarWeekViewModel? CalendarWeekViewModel { get; private set; }

        private CalendarWeekViewModel Calendar =>
            CalendarWeekViewModel ?? throw new InvalidOperationException("The calendar has not been initialized.");

        public DateTime CurrentWeek => Calendar.WeekStart;

        public string WeekDisplayText =>
            $"{CurrentWeek:MMMM d} - {CurrentWeek.AddDays(6):MMMM d}";


        // menu commands
        public ICommand ManageShowsCommand { get; }

        public ICommand ManageArtistsCommand { get; }

        public ICommand ManageEmployeesCommand { get; }

        public ICommand ManageTicketholdersCommand { get; }

        public ICommand SellTicketCommand { get; }

        public ICommand ReportsCommand { get; }

        public ICommand ExitCommand { get; }


        // calendar commands
        public ICommand PreviousWeekCommand { get; }

        public ICommand NextWeekCommand { get; }

        public ICommand TodayCommand { get; }


        // constructor
        public MainWindowViewModel(
            ShowDAC showDAC, 
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider 
                ?? throw new ArgumentNullException(nameof(serviceProvider));

            _showDAC = showDAC  
                ?? throw new ArgumentNullException(nameof(showDAC));


            #region menu commands
            ManageShowsCommand =
                new RelayCommand(ManageShows);

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

        // menu actions
        private void ManageShows()
        {
            MessageBox.Show("Manage Shows");
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
    }
}