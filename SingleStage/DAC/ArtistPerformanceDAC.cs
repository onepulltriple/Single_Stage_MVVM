using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class ArtistPerformanceDAC : BaseDAC<ArtistPerformance>
    {
        public ArtistPerformanceDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
