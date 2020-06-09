using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private WolfMonitorContext _context;

        public ProfileRepository(WolfMonitorContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, Profile>> CreateAsync(Profile entity)
        {
            EntityEntry<Profile> newEntity = _context.AgentProfiles.Add(entity);

            await _context.SaveChangesAsync();

            return newEntity.Entity;
        }

        public Result<Exception, IQueryable<Profile>> GetAll()
            => Result.Run(() => _context.AgentProfiles.AsNoTracking().Where(a => !a.Removed));

        public Result<Exception, IQueryable<Profile>> GetAllByAgentId(Guid agentId)
            => Result.Run(() => _context.AgentProfiles.AsNoTracking().Where(a => !a.Removed && a.AgentId == agentId));

        public Result<Exception, IQueryable<Profile>> GetAllByProfileIdentifier(Guid agentId, Guid profileIdentifier)
            => Result.Run(() => _context.AgentProfiles.AsNoTracking().Where(a => !a.Removed && a.AgentId == agentId && a.ProfileIdentifier == profileIdentifier));

        public async Task<Result<Exception, Profile>> GetByIdAsync(Guid id)
        {
            var entity = await _context.AgentProfiles.AsNoTracking().FirstOrDefaultAsync(a => !a.Removed && a.Id == id);

            if (entity == null)
                return new NotFoundException();

            return entity;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Profile entity)
        {
            entity.UpdatedIn = DateTime.Now;

            _context.AgentProfiles.Update(entity);

            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
