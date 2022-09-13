namespace MetricsAgent.Models.Requests
{
    public class RamAvailableMetricCreateRequest
    {
        public int Value { get; set; }

        public TimeSpan Time { get; set; }
    }
}
