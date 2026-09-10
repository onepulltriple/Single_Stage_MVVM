using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;
using SingleStage.DAC.Interfaces;

namespace SingleStage.DAC
{
    public class ArtistDAC : BaseDAC<Artist>, IArtistDAC
    {
        public ArtistDAC(SingleStageMvvmContext context) : base(context) { }
        
        public async Task<int> GetPerformanceCountAsync(int artistId)
        {
            return await _context.ArtistPerformances
                .CountAsync(ap => ap.ArtistId == artistId);
        }
    }
}
