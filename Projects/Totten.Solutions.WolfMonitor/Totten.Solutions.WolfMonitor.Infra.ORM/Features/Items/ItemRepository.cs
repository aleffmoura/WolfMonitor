using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items
{
    public class ItemRepository : IItemRepository
    {
        private WolfMonitoringContext _context;

        public ItemRepository(WolfMonitoringContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, Item>> CreateAsync(Item entity)
        {
            EntityEntry<Item> newitem = _context.Items.Add(entity);

            await _context.SaveChangesAsync();

            return newitem.Entity;
        }
        public async Task<Result<Exception, Unit>> CreateHistoricAsync(ItemHistoric itemHistoric)
        {
            _context.Historic.Add(itemHistoric);

            await _context.SaveChangesAsync();

            return Unit.Successful;
        }

        public async Task<Result<Exception, Unit>> CreateSolicitationAsync(ItemSolicitationHistoric item)
        {
            _context.SolicitationsHistoric.Add(item);

            await _context.SaveChangesAsync();

            return Unit.Successful;
        }

        public Result<Exception, IQueryable<Item>> GetAll()
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed));

        public Result<Exception, IQueryable<Item>> GetAll(Guid agentId)
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed && item.AgentId == agentId));

        public Result<Exception, IQueryable<Item>> GetAll(Guid agentId, ETypeItem eTypeItem)
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed && item.AgentId == agentId && item.Type == eTypeItem));

        public Result<Exception, IQueryable<Item>> GetAllByCompanyId(Guid companyId)
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed && item.CompanyId == companyId));

        public Result<Exception, IQueryable<ItemHistoric>> GetAllHistoric(Guid itemId)
            => Result.Run(() => _context.Historic.AsNoTracking().OrderByDescending(x => x.MonitoredAt).Where(item => !item.Removed && item.ItemId == itemId));

        public Result<Exception, IQueryable<ItemSolicitationHistoric>> GetAllSolicitation(Guid itemId)
            => Result.Run(() => _context.SolicitationsHistoric.AsNoTracking().Where(item => !item.Removed && item.ItemId == itemId));

        public async Task<Result<Exception, Item>> GetByIdAsync(Guid id)
        {
            Item Item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(item => !item.Removed && item.Id == id);

            if (Item == null)
                return new NotFoundException("Não foi encontrado um item com o identificador informado.");

            return Item;
        }

        public async Task<Result<Exception, Item>> GetByIdAsync(Guid agentId, Guid id)
        {
            Item Item = await _context.Items.AsNoTracking()
                                             .FirstOrDefaultAsync(item => !item.Removed && item.AgentId == agentId && item.Id == id);

            if (Item == null)
                return new NotFoundException("Não foi encontrado um item com o identificador informado.");

            return Item;
        }

        public async Task<Result<Exception, Item>> GetByNameOrDisplayNameWithAgentId(Guid agentId, string name, string displayName )
        {
            Item Item = await _context.Items.AsNoTracking()
                                            .FirstOrDefaultAsync(item => !item.Removed && item.AgentId == agentId &&
                                                                         (item.Name.Equals(name) || item.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)));

            if (Item == null)
                return new NotFoundException("Não foi encontrado um item com o identificador do agent informado.");

            return Item;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Item entity)
        {
            entity.UpdatedIn = DateTime.Now;
            _context.Items.Update(entity);
            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
