namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents
{
    public class AgentDetailViewModel
    {
        public string Id { get; set; }
        public string ProfileName { get; set; }
        public string DisplayName { get; set; }
        public string MachineName { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string CreatedIn { get; set; }
        public string FirstConnection { get; set; }
        public string LastConnection { get; set; }
        public string LastUpload { get; set; }
        public bool Configured { get; set; }
    }
}
