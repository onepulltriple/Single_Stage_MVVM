using SingleStage.Entities;

namespace SingleStage.Infrastructure.EventArguments
{
    public class DeleteShowConfirmationEventArguments : EventArgs
    {
        public Show Show { get; }

        public int PerformanceCount { get; }

        public int TicketCount { get; }

        public bool Confirmed { get; set; }

        public DeleteShowConfirmationEventArguments(
            Show show,
            int performanceCount,
            int ticketCount )
        {
            Show = show;
            PerformanceCount = performanceCount;
            TicketCount = ticketCount;
        }
    }
}
