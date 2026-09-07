using SingleStage.Entities;
using SingleStage.Calendar;

namespace SingleStage.Infrastructure
{
    public class ShowScheduleValidator
    {
        public Show? GetConflict(Show show, IEnumerable<Show> existingShows)
        {
            return existingShows.FirstOrDefault(existing =>
                existing.Id != show.Id &&
                show.StartTime < existing.EndTime &&
                show.EndTime > existing.StartTime);
        }

        public bool IsWithinOpeningHours(Show show)
        {
            DateTime openingTime = show.StartTime.Date.AddHours(CalendarLayout.StartHour);
            DateTime closingTime = show.StartTime.Date.AddHours(CalendarLayout.EndHour + 1); // closes at end of listed hour

            return show.StartTime >= openingTime &&
                   show.EndTime <= closingTime;
        }
    }
}
