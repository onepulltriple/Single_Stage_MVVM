using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class AppearanceDAC : BaseDAC<Appearance>
    {
        public AppearanceDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
