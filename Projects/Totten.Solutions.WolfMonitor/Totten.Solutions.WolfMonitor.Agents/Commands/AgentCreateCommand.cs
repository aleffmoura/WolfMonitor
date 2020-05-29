namespace Totten.Solutions.WolfMonitor.Agents.Commands
{
    public class AgentCreateCommand
    {
        public string DisplayName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool ReadItemsMonitoringByArchive { get; set; }
    }
}
