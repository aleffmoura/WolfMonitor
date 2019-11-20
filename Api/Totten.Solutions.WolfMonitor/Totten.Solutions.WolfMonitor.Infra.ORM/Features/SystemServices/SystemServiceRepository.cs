using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.SystemServices
{
    public class SystemServiceRepository : ISystemServiceRepository
    {
        private WolfMonitoringContext _context;

        public SystemServiceRepository(WolfMonitoringContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, SystemService>> CreateAsync(SystemService entity)
        {
            EntityEntry<SystemService> newService = _context.SystemServices.Add(entity);

            await _context.SaveChangesAsync();

            return newService.Entity;
        }

        public Result<Exception, IQueryable<SystemService>> GetAll()
        {
            return Result.Run(() => _context.SystemServices.AsNoTracking().Where(service => !a.Removed));
        }

        public async Task<Result<Exception, SystemService>> GetByIdAsync(Guid id)
        {
            SystemService systemService = await _context.SystemServices.AsNoTracking().FirstOrDefaultAsync(service => !service.Removed && service.Id == id);

            if (systemService == null)
            {
                return new NotFoundException();
            }
            return systemService;
        }

        public async Task<Result<Exception, SystemService>> GetByNameWithAgentId(string name, Guid agentId)
        {
            SystemService systemService = await _context.SystemServices
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(service => !service.Removed && service.Name.Equals(name) && service.AgentId == agentId);

            if (systemService == null)
            {
                return new NotFoundException();
            }
            return systemService;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(SystemService entity)
        {
            entity.UpdatedIn = DateTime.Now;
            _context.SystemServices.Update(entity);
            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
