using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Agents.Commands.Profiles
{
    public class ProfileApplyCommand
    {
        public Guid ProfileIdentifier { get; set; }
        public Guid AgentId { get; set; }
    }
}
