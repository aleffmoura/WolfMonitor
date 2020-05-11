namespace Totten.Solutions.WolfMonitor.Agents.Commands
{
    public class AgentUpdateCommand
    {
        public string MachineName { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
    }
}
