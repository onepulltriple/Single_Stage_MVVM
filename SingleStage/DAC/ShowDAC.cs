using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class ShowDAC : BaseDAC<Show>
    {
        public ShowDAC(SingleStageMvvmContext context) : base(context) { }

        public async Task<int> GetPerformanceCountAsync(int showId)
        {
            return await _context.Performances
                .CountAsync(p => p.ShowId == showId);
        }
        public async Task<int> GetTicketCountAsync(int showId)
        {
            return await _context.Tickets
                .CountAsync(t => t.ShowId == showId);
        }
    }
}