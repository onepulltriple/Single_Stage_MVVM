using SingleStage.Entities;

namespace SingleStage.Infrastructure.EventArguments
{
    public class DeleteArtistConfirmationEventArguments : EventArgs
    {
        public Artist Artist { get; }
        public int PerformanceCount { get; }

        public bool Confirmed { get; set; }

        public DeleteArtistConfirmationEventArguments(
            Artist artist,
            int performanceCount)
        {
            Artist = artist;
            PerformanceCount = performanceCount;
        }
    }

}
