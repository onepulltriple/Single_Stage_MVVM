using SingleStage.Entities;

namespace SingleStage.Infrastructure.EventArguments
{
    public class DeletePerformanceConfirmationEventArguments : EventArgs
    {
        public Performance Performance { get; }
        public int ArtistPerformanceCount { get; }

        public bool Confirmed { get; set; }

        public DeletePerformanceConfirmationEventArguments(
            Performance performance,
            int artistPerformanceCount)
        {
            Performance = performance;
            ArtistPerformanceCount = artistPerformanceCount;
        }
    }
}
