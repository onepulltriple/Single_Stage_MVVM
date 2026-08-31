using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class PerformanceDAC : BaseDAC<Performance>
    {
        public PerformanceDAC(SingleStageMvvmContext context) : base(context) { }

        // include Show so the UI can display the show name
        public override async Task<List<Performance>> GetAllAsync()
        {
            return await _context.Set<Performance>()
                .Include(p => p.Show)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Performance?> GetByIdAsync(int id)
        {
            return await _context.Set<Performance>()
                .Include(p => p.Show)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
