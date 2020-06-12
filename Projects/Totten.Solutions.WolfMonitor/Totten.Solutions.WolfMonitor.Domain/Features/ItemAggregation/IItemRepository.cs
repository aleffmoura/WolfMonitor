using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Result<Exception, Unit>> CreateHistoricAsync(ItemHistoric item);
        Task<Result<Exception, Unit>> CreateSolicitationAsync(ItemSolicitationHistoric item);
        Task<Result<Exception, Item>> GetByNameOrDisplayNameWithAgentId(Guid agentId, string name, string displayName);
        Result<Exception, IQueryable<Item>> GetAll(Guid agentId);
        Result<Exception, IQueryable<Item>> GetAllByCompanyId(Guid companyId);
        Result<Exception, IQueryable<Item>> GetAll(Guid agentId, ETypeItem type);
        Result<Exception, IQueryable<ItemHistoric>> GetAllHistoric(Guid itemId);
        Result<Exception, IQueryable<ItemSolicitationHistoric>> GetAllSolicitation(Guid itemId);
        Task<Result<Exception, Item>> GetByIdAsync(Guid agentId, Guid id);
        
    }
}
