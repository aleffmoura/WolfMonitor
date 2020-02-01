using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            EntityEntry<Item> newService = _context.Items.Add(entity);

            await _context.SaveChangesAsync();

            return newService.Entity;
        }
        public Result<Exception, IQueryable<Item>> GetAll()
        {
            return Result.Run(() => _context.Items.AsNoTracking().Where(service => !service.Removed));
        }

        public Result<Exception, IQueryable<Item>> GetAll(Guid agentId)
        {
            return Result.Run(() => _context.Items.AsNoTracking().Where(service => !service.Removed && service.AgentId == agentId));
        }

        public async Task<Result<Exception, Item>> GetByIdAsync(Guid id)
        {
            Item Item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(service => !service.Removed && service.Id == id);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um serviço com o identificador informado.");
            }
            return Item;
        }
        public async Task<Result<Exception, Item>> GetByIdAsync(Guid agentId, Guid id)
        {
            Item Item = await _context.Items.AsNoTracking()
                                             .FirstOrDefaultAsync(service => !service.Removed && service.AgentId == agentId && service.Id == id);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um serviço com o identificador informado.");
            }
            return Item;
        }

        public async Task<Result<Exception, Item>> GetByNameWithAgentId(string name, Guid agentId)
        {
            Item Item = await _context.Items
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(service => !service.Removed && service.Name.Equals(name) && service.AgentId == agentId);

            if (Item == null)
            {
                return new NotFoundException("Não foi encontrado um serviço com o identificador do agent informado.");
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
