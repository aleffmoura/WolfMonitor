using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Result<Exception, Item>> GetByNameWithAgentId(string name, Guid agentId);
        Result<Exception, IQueryable<Item>> GetAll(Guid agentId);
        Task<Result<Exception, Item>> GetByIdAsync(Guid agentId, Guid id);
    }
}
