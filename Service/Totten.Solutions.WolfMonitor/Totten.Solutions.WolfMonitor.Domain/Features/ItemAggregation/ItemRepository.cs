using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public interface ItemRepository
    {
        public Task<Guid> AddAsync(Item item);
        public Task<List<Item>> GetAll(Guid agentId, EType type);
    }
}
