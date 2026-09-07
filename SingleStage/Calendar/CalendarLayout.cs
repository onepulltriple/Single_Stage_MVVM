namespace SingleStage.Calendar
{
    public static class CalendarLayout
    {
        public const double PixelsPerHour = 45.0;

        public const double TotalHeight = PixelsPerHour * 24.0;
        
        public const double ShowWidth = 180.0;

        public const int StartHour = 10;
        public const int EndHour = 23;

        public const int NumberOfHours = EndHour - StartHour + 1;
    }
}