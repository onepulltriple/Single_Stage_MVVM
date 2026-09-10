using SingleStage.Entities;

namespace SingleStage.Infrastructure.EventArguments
{
    public class DeleteTicketholderConfirmationEventArguments : EventArgs
    {
        public Ticketholder Ticketholder { get; }
        public int TicketCount { get; }

        public bool Confirmed { get; set; }

        public DeleteTicketholderConfirmationEventArguments(
            Ticketholder ticketholder,
            int ticketCount)
        {
            Ticketholder = ticketholder;
            TicketCount = ticketCount;
        }
    }
}
