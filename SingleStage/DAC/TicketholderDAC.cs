using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class TicketholderDAC : BaseDAC<Ticketholder>
    {
        public TicketholderDAC(SingleStageMvvmContext context) : base(context) { }

        public async Task<int> GetTicketCountAsync(int ticketholderId)
        {
            return await _context.Tickets
                .CountAsync(t => t.TicketholderId == ticketholderId);
        }
    }
}
