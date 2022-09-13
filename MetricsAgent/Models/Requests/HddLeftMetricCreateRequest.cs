namespace MetricsAgent.Models.Requests
{
    public class HddLeftMetricCreateRequest
    {
        public int Value { get; set; }

        public TimeSpan Time { get; set; }
    }
}
