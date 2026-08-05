using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class TicketholderDAC : BaseDAC<Ticketholder>
    {
        public TicketholderDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
