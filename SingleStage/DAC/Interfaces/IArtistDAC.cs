using SingleStage.Entities;

namespace SingleStage.DAC.Interfaces
{
    public interface IArtistDAC : IBaseDAC<Artist>
    {
        Task<int> GetPerformanceCountAsync(int artistId);
    }
}
