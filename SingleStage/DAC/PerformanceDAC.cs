using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class PerformanceDAC : BaseDAC<Performance>
    {
        public PerformanceDAC(SingleStageMvvmContext context) : base(context) { }
    }
}
