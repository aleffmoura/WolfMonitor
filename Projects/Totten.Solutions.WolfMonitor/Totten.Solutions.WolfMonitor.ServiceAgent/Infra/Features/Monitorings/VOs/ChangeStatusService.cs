using System;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs
{
    public enum SolicitationType
    {
        ChangeStatus = 0,
        ChangeKeyFile = 1,
        ChangeValueFile = 2,
        ChangeFile = 3
    }
    public class ChangeStatusService
    {
        public Guid ItemId { get; set; }
        public Guid AgentId { get; set; }
        public SolicitationType SolicitationType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewStatus { get; set; }
    }
}
