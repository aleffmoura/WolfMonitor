using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles
{
    public interface IProfileRepository : IRepository<Profile>
    {
        Result<Exception, IQueryable<Profile>> GetAllByAgentId(Guid agentId);
        Result<Exception, IQueryable<Profile>> GetAllByProfileIdentifier(Guid agentId, Guid profileIdentifier);
    }
}
