using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public interface IItemsRepository : IRepository<Items>
    {
        Task<Result<Exception, List<Items>>> GetByAgentIdAsync(Guid agentId);
        Task<Result<Exception, Unit>> CreateWithUnitReturnAsync(Items item);
    }
}
