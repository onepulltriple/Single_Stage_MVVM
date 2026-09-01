using SingleStage.Entities;

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
    }
}
