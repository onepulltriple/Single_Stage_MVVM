using SingleStage.Calendar;
using SingleStage.Entities;
using SingleStage.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SingleStage.ViewModels
{
    public class CalendarWeekViewModel : ViewModelBase
    {
        private readonly List<Show> _shows;

        private DateTime _calendarStartDate;

        public DateTime CalendarStartDate
        {
            get => _calendarStartDate;
            set
            {
                DateTime newCalendarStartDate = value.Date;

                if (_calendarStartDate == newCalendarStartDate)
                    return;

                _calendarStartDate = newCalendarStartDate;

                RefreshShows();

                OnPropertyChanged(nameof(CalendarStartDate));
                OnPropertyChanged(nameof(FirstDayHeader));
                OnPropertyChanged(nameof(SecondDayHeader));
                OnPropertyChanged(nameof(ThirdDayHeader));
                OnPropertyChanged(nameof(FourthDayHeader));
                OnPropertyChanged(nameof(FifthDayHeader));
                OnPropertyChanged(nameof(SixthDayHeader));
                OnPropertyChanged(nameof(SeventhDayHeader));
            }
        }

        private Show? _selectedShow;

        public Show? SelectedShow
        {
            get => _selectedShow;
            set
            {
                if (_selectedShow == value)
                    return;

                _selectedShow = value;
                OnPropertyChanged(nameof(SelectedShow));
            }
        }


        public ObservableCollection<CalendarHourViewModel> Hours { get; } = new();

        public ObservableCollection<CalendarShowViewModel> FirstDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> SecondDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> ThirdDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> FourthDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> FifthDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> SixthDay { get; } = new();

        public ObservableCollection<CalendarShowViewModel> SeventhDay { get; } = new();


        public RelayCommand SelectShowCommand { get; }

        public RelayCommand OpenShowCommand { get; }

        public event EventHandler<int>? OpenShowRequested;


        #region calendar day headers
        public string FirstDayHeader =>
            _calendarStartDate.ToString("ddd dd MMM");

        public string SecondDayHeader =>
            _calendarStartDate.AddDays(1).ToString("ddd dd MMM");

        public string ThirdDayHeader =>
            _calendarStartDate.AddDays(2).ToString("ddd dd MMM");

        public string FourthDayHeader =>
            _calendarStartDate.AddDays(3).ToString("ddd dd MMM");

        public string FifthDayHeader =>
            _calendarStartDate.AddDays(4).ToString("ddd dd MMM");

        public string SixthDayHeader =>
            _calendarStartDate.AddDays(5).ToString("ddd dd MMM");

        public string SeventhDayHeader =>
            _calendarStartDate.AddDays(6).ToString("ddd dd MMM");

        #endregion


        // constructor
        public CalendarWeekViewModel(DateTime calendarStartDate, IEnumerable<Show> shows)
        {
            _shows = shows.ToList();

            SelectShowCommand = new RelayCommand(SelectShow);

            OpenShowCommand = new RelayCommand(OpenShow);

            CreateHours();

            _calendarStartDate = calendarStartDate.Date;

            RefreshShows();
        }

        private void SelectShow(object? parameter)
        {
            if (parameter is not CalendarShowViewModel showViewModel)
                return;

            SelectedShow = showViewModel.Show;
        }

        private void OpenShow(object? parameter)
        {
            if (parameter is CalendarShowViewModel showViewModel)
            {
                OpenShowRequested?.Invoke(this, showViewModel.Show.Id);
            }
        }

        private void RefreshShows()
        {
            FirstDay.Clear();
            SecondDay.Clear();
            ThirdDay.Clear();
            FourthDay.Clear();
            FifthDay.Clear();
            SixthDay.Clear();
            SeventhDay.Clear();

            DateTime calendarEndDate = _calendarStartDate.AddDays(7);

            IEnumerable<Show> showsThisWeek =
                _shows.Where(show =>
                    show.StartTime >= _calendarStartDate &&
                    show.StartTime < calendarEndDate);

            foreach (Show show in showsThisWeek)
            {
                var vm = new CalendarShowViewModel(show);

                int dayIndex = (show.StartTime.Date - _calendarStartDate.Date).Days;

                switch (dayIndex)
                {
                    case 0:
                        FirstDay.Add(vm);
                        break;

                    case 1:
                        SecondDay.Add(vm);
                        break;

                    case 2:
                        ThirdDay.Add(vm);
                        break;

                    case 3:
                        FourthDay.Add(vm);
                        break;

                    case 4:
                        FifthDay.Add(vm);
                        break;

                    case 5:
                        SixthDay.Add(vm);
                        break;

                    case 6:
                        SeventhDay.Add(vm);
                        break;
                }
            }
        }


        private void CreateHours()
        {
            for (int hour = CalendarLayout.StartHour;
                 hour <= CalendarLayout.EndHour;
                 hour++)
            {
                Hours.Add(new CalendarHourViewModel(hour));
            }
        }
    }
}