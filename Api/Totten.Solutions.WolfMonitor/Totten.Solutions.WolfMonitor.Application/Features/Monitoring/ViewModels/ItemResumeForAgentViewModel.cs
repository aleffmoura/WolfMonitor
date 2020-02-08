using System;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices
{
    public class ItemResumeForAgentViewModel
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Interval { get; set; }
        public string Default { get; set; }
        public string Value { get; set; }
        public string LastValue { get; set; }
        public ETypeItem Type { get; set; }
    }
}
