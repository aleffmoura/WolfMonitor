using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public interface IAgentRepository : IRepository<Agent>
    {
        Result<Exception, IQueryable<Agent>> GetAll(Guid companyId);
        Result<Exception, IQueryable<Agent>> GetAllByUserId(Guid userId);
        Task<Result<Exception, Agent>> GetByIdAsync(Guid companyId, Guid id);
        Task<Result<Exception, Agent>> GetByLoginOrDisplayName(Guid companyId, string login, string displayName);
        Task<Result<Exception, Agent>> Authentication(Guid companyId, string login, string password);
    }
}
