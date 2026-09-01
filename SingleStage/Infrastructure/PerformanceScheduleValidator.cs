using SingleStage.Entities;

namespace SingleStage.Infrastructure
{
    public class PerformanceScheduleValidator
    {
        public string? Validate(
            Performance performance,
            IEnumerable<Performance> existingPerformances,
            IEnumerable<Show> shows)
        {
            Show? show = shows.FirstOrDefault(s => s.Id == performance.ShowId);

            if (show is null)
                return "The selected show could not be found.";

            if (performance.StartTime < show.StartTime ||
                performance.EndTime > show.EndTime)
            {
                return
                    $"The performance must be within the selected show's timeframe " +
                    $"({show.StartTime:ddd MMM d HH:mm} - {show.EndTime:ddd MMM d HH:mm}).";
            }

            Performance? conflictingPerformance = existingPerformances.FirstOrDefault(existing =>
                existing.Id != performance.Id &&
                existing.ShowId == performance.ShowId &&
                performance.StartTime < existing.EndTime &&
                performance.EndTime > existing.StartTime);

            if (conflictingPerformance is not null)
            {
                return
                    $"The performance overlaps with \"{conflictingPerformance.Description}\" " +
                    $"({conflictingPerformance.StartTime:HH:mm} - " +
                    $"{conflictingPerformance.EndTime:HH:mm}).";
            }

            return null;
        }
    }
}
