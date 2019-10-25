using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Result<Exception, Item>> GetByAgentIdAsync(Guid agentId);
    }
}
