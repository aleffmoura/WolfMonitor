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

        public Result<Exception, IQueryable<Item>> GetAll()
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed));
        public Result<Exception, IQueryable<Item>> GetAllWithHistoric()
            => Result.Run(() => _context.Items.Include(x => x.Historic).AsNoTracking().Where(item => !item.Removed));

        public Result<Exception, IQueryable<Item>> GetAll(Guid agentId)
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed && item.AgentId == agentId));

        public Result<Exception, IQueryable<Item>> GetAll(Guid agentId, ETypeItem eTypeItem)
            => Result.Run(() => _context.Items.AsNoTracking().Where(item => !item.Removed && item.AgentId == agentId && item.Type == eTypeItem));

        public async Task<Result<Exception, Item>> GetByIdAsync(Guid id)
        {
            Item Item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(item => !item.Removed && item.Id == id);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um item com o identificador informado.");
            }
            return Item;
        }

        public async Task<Result<Exception, Item>> GetByIdAsync(Guid agentId, Guid id)
        {
            Item Item = await _context.Items.AsNoTracking()
                                             .FirstOrDefaultAsync(item => !item.Removed && item.AgentId == agentId && item.Id == id);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um item com o identificador informado.");
            }
            return Item;
        }

        public async Task<Result<Exception, Item>> GetByNameWithAgentId(string name, Guid agentId)
        {
            Item Item = await _context.Items.AsNoTracking()
                                            .FirstOrDefaultAsync(item => !item.Removed && item.Name.Equals(name) && item.AgentId == agentId);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um item com o identificador do agent informado.");
            }
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
