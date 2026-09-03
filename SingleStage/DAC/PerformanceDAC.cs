using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class PerformanceDAC : BaseDAC<Performance>
    {
        public PerformanceDAC(SingleStageMvvmContext context) : base(context) { }

        // include Show and ArtistPerformance/Artist so the UI 
        // can display all information belonging to each performance
        public override async Task<List<Performance>> GetAllAsync()
        {
            return await _context.Set<Performance>()
                .Include(p => p.Show)
                .Include(p => p.ArtistPerformances)
                    .ThenInclude(ap => ap.Artist)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Performance?> GetByIdAsync(int id)
        {
            return await _context.Set<Performance>()
                .Include(p => p.Show)
                .Include(p => p.ArtistPerformances)
                    .ThenInclude(ap => ap.Artist)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
