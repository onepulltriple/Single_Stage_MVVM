using SingleStage.Entities;
using SingleStage.Calendar;

namespace SingleStage.ViewModels
{
    public class CalendarShowViewModel : ViewModelBase
    {
        private const double PixelsPerHour = CalendarLayout.PixelsPerHour;

        public Show Show { get; }

        public CalendarShowViewModel(Show show)
        {
            Show = show;
        }

        public int Id => Show.Id;

        public string Name => Show.Name;

        public DateTime StartTime => Show.StartTime;

        public DateTime EndTime => Show.EndTime;

        public string TimeText =>
            $"{StartTime:HH:mm} - {EndTime:HH:mm}";

        /// <summary>
        /// Pixel offset from midnight.
        /// </summary>
        public double Top =>
            StartTime.TimeOfDay.TotalHours * PixelsPerHour;

        /// <summary>
        /// Pixel height of the show block.
        /// </summary>
        public double Height =>
            (EndTime - StartTime).TotalHours * PixelsPerHour;

        /// <summary>
        /// Monday = 0 ... Sunday = 6
        /// </summary>
        public int DayIndex
        {
            get
            {
                int day = (int)StartTime.DayOfWeek;

                return day == 0
                    ? 6
                    : day - 1;
            }
        }
    }
}
