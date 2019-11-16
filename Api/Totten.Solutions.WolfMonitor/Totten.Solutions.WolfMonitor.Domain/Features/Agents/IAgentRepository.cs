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
        Task<Result<Exception, Agent>> GetByIdAsync(Guid companyId, Guid id);
        Task<Result<Exception, Agent>> GetByLogin(Guid companyId, string login);
        Task<Result<Exception, Agent>> Authentication(Guid companyId, string login, string password);
    }
}
