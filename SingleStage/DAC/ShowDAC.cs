using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class ShowDAC : BaseDAC<Show>
    {
        public ShowDAC(SingleStageMvvmContext context) : base(context) { }
    }
}