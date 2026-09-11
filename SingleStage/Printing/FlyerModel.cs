namespace SingleStage.Models
{
    public class FlyerModel
    {
        public string ShowName { get; init; } = string.Empty;

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public decimal? TicketPrice { get; init; }

        public List<FlyerPerformanceModel> Performances { get; init; } = new();
    }

    public class FlyerPerformanceModel
    {
        public string Description { get; init; } = string.Empty;

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public List<string> Artists { get; init; } = new();
    }
}
