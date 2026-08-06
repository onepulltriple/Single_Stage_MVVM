using SingleStage.Calendar;

namespace SingleStage.ViewModels
{
    public class CalendarHourViewModel
    {
        public string Label { get; }
        public double Top { get; }

        public CalendarHourViewModel(int hour)
        {
            Label = $"{hour:00}:00";
            Top = hour * CalendarLayout.PixelsPerHour;
        }
    }
}
