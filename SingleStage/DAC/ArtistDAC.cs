using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class ArtistDAC : BaseDAC<Artist>
    {
        public ArtistDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
