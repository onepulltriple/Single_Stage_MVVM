using System;
using System.Globalization;

namespace SingleStage.Infrastructure
{
    public static class DateTimeHelper
    {
        public static bool TryParseTime(string text, out TimeSpan time)
        {
            time = default;

            if (string.IsNullOrWhiteSpace(text))
                return false;

            string trimmedText = text.Trim();

            if (trimmedText == "24:00")
            {
                time = TimeSpan.FromHours(24);
                return true;
            }

            if (TimeSpan.TryParse(trimmedText, out time))
                return true;

            if (DateTime.TryParse(trimmedText, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                time = dateTime.TimeOfDay;
                return true;
            }

            return false;
        }

        public static bool TryCombineDateAndTime(DateTime? date, string timeText, out DateTime result)
        {
            result = default;

            if (date is null)
                return false;

            if (!TryParseTime(timeText, out TimeSpan time))
                return false;

            result = date.Value.Date + time;

            return true;
        }
    }
}
