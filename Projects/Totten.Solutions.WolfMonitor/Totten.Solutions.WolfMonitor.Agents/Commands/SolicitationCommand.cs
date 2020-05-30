using System;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Agents.Commands
{
    public class SolicitationCommand
    {
        public Guid ItemId { get; set; }
        public Guid AgentId { get; set; }
        public SolicitationType SolicitationType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewValue { get; set; }
    }
}
