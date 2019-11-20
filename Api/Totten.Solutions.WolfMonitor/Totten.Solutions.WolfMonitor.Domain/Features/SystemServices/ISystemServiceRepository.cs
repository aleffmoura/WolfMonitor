using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.SystemServices
{
    public interface ISystemServiceRepository : IRepository<SystemService>
    {
        Task<Result<Exception, SystemService>> GetByNameWithAgentId(string name, Guid agentId);
    }
}
