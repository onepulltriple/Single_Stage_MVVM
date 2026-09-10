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

        private DateTime _weekStart;

        public DateTime WeekStart
        {
            get => _weekStart;
            set
            {
                DateTime newWeekStart = value.Date;

                if (_weekStart == newWeekStart)
                    return;

                _weekStart = newWeekStart;

                RefreshShows();

                OnPropertyChanged(nameof(WeekStart));
                OnPropertyChanged(nameof(MondayHeader));
                OnPropertyChanged(nameof(TuesdayHeader));
                OnPropertyChanged(nameof(WednesdayHeader));
                OnPropertyChanged(nameof(ThursdayHeader));
                OnPropertyChanged(nameof(FridayHeader));
                OnPropertyChanged(nameof(SaturdayHeader));
                OnPropertyChanged(nameof(SundayHeader));
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

        public ObservableCollection<CalendarShowViewModel> Monday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Tuesday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Wednesday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Thursday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Friday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Saturday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Sunday { get; } = new();

        public ICommand SelectShowCommand { get; }


        #region calendar day headers
        public string MondayHeader =>       _weekStart.ToString("ddd dd MMM");

        public string TuesdayHeader =>      _weekStart.AddDays(1).ToString("ddd dd MMM");

        public string WednesdayHeader =>    _weekStart.AddDays(2).ToString("ddd dd MMM");

        public string ThursdayHeader =>     _weekStart.AddDays(3).ToString("ddd dd MMM");

        public string FridayHeader =>       _weekStart.AddDays(4).ToString("ddd dd MMM");

        public string SaturdayHeader =>     _weekStart.AddDays(5).ToString("ddd dd MMM");

        public string SundayHeader =>       _weekStart.AddDays(6).ToString("ddd dd MMM");
        #endregion


        // constructor
        public CalendarWeekViewModel(DateTime weekStart, IEnumerable<Show> shows)
        {
            _shows = shows.ToList();

            SelectShowCommand = new RelayCommand(SelectShow);

            CreateHours();

            _weekStart = weekStart.Date;

            RefreshShows();
        }

        private void SelectShow(object? parameter)
        {
            if (parameter is not CalendarShowViewModel showViewModel)
                return;

            SelectedShow = showViewModel.Show;
        }


        private void RefreshShows()
        {
            Monday.Clear();
            Tuesday.Clear();
            Wednesday.Clear();
            Thursday.Clear();
            Friday.Clear();
            Saturday.Clear();
            Sunday.Clear();

            DateTime weekEnd = _weekStart.AddDays(7);

            IEnumerable<Show> showsThisWeek =
                _shows.Where(show =>
                show.StartTime >= _weekStart &&
                show.StartTime < weekEnd);

            foreach (Show show in showsThisWeek)
            {
                var vm = new CalendarShowViewModel(show);

                switch (vm.DayIndex)
                {
                    case 0:
                        Monday.Add(vm);
                        break;

                    case 1:
                        Tuesday.Add(vm);
                        break;

                    case 2:
                        Wednesday.Add(vm);
                        break;

                    case 3:
                        Thursday.Add(vm);
                        break;

                    case 4:
                        Friday.Add(vm);
                        break;

                    case 5:
                        Saturday.Add(vm);
                        break;

                    case 6:
                        Sunday.Add(vm);
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