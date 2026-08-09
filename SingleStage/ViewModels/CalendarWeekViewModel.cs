using System;
using System.Collections.ObjectModel;
using System.Linq;
using SingleStage.Entities;

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
            }
        }

        public Show? SelectedShow { get; set; }

        public ObservableCollection<CalendarHourViewModel> Hours { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Monday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Tuesday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Wednesday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Thursday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Friday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Saturday { get; } = new();

        public ObservableCollection<CalendarShowViewModel> Sunday { get; } = new();


        public CalendarWeekViewModel(DateTime weekStart, IEnumerable<Show> shows)
        {
            _shows = shows.ToList();

            CreateHours();

            _weekStart = weekStart.Date;

            RefreshShows();
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
            for (int i = 0; i < 24; i++)
            {
                Hours.Add(new CalendarHourViewModel(i));
            }
        }
    }
}