using SingleStage.Entities;

namespace SingleStage.Infrastructure.EventArguments
{
    public class DeleteShowConfirmationEventArguments : EventArgs
    {
        public Show Show { get; }

        public int PerformanceCount { get; }

        public bool Confirmed { get; set; }

        public DeleteShowConfirmationEventArguments(
            Show show,
            int performanceCount)
        {
            Show = show;
            PerformanceCount = performanceCount;
        }
    }
}
