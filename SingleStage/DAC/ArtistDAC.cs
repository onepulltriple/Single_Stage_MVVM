using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;
using SingleStage.DAC.Interfaces;

namespace SingleStage.DAC
{
    public class ArtistDAC : BaseDAC<Artist>, IArtistDAC
    {
        public ArtistDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
