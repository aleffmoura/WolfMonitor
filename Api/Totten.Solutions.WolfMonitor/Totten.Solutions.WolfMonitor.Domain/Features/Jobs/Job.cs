namespace Totten.Solutions.WolfMonitor.Domain.Features.Jobs
{
    public class Job
    {
        public int IntervalMinutes { get; set; }
        public string TargetServiceName { get; set; }
        public string MessageType { get; set; }
    }
}
