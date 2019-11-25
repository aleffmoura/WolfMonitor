using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.SystemServices
{
    public interface ISystemServiceRepository : IRepository<SystemService>
    {
        Task<Result<Exception, SystemService>> GetByNameWithAgentId(string name, Guid agentId);
        Result<Exception, IQueryable<SystemService>> GetAll(Guid agentId);
        Task<Result<Exception, SystemService>> GetByIdAsync(Guid agentId, Guid id);
    }
}
