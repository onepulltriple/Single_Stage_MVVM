using System;
using System.Collections.ObjectModel;
using System.Linq;
using SingleStage.Entities;

namespace SingleStage.ViewModels
{
    public class CalendarWeekViewModel : ViewModelBase
    {
        public DateTime WeekStart { get; }

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
            CreateHours();

            WeekStart = weekStart.Date;

            foreach (Show show in shows)
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