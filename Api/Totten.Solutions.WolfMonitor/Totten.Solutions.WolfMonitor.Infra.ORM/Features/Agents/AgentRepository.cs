using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents
{
    public class AgentRepository : IAgentRepository
    {
        private WolfMonitorContext _context;

        public AgentRepository(WolfMonitorContext context)
        {
            _context = context;
        }
        public async Task<Result<Exception, Agent>> CreateAsync(Agent agent)
        {
            EntityEntry<Agent> newAgent = _context.Agents.Add(agent);

            await _context.SaveChangesAsync();

            return newAgent.Entity;
        }
        public async Task<Result<Exception, Agent>> GetByIdAsync(Guid id)
        {
            Agent agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (agent == null)
            {
                return new NotFoundException();
            }

            return agent;
        }
        public async Task<Result<Exception, Agent>> GetByLogin(Guid companyId, string login)
        {
            Agent agent = await _context.Agents
                                        .FirstOrDefaultAsync(a => !a.Removed && a.CompanyId == companyId && a.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase));

            if (agent == null)
            {
                return new NotFoundException();
            }

            return agent;
        }
        public async Task<Result<Exception, Agent>> GetByIdAsync(Guid companyId, Guid id)
        {
            Agent agent = await _context.Agents.FirstOrDefaultAsync(a => !a.Removed && a.Id == id && a.CompanyId == companyId);

            if (agent == null)
            {
                return new NotFoundException();
            }
            return agent;
        }
        public Result<Exception, IQueryable<Agent>> GetAll()
        {
            return Result.Run(() => _context.Agents.Where(a => !a.Removed));
        }
        public Result<Exception, IQueryable<Agent>> GetAll(Guid companyId)
        {
            return Result.Run(() => _context.Agents.Where(a => !a.Removed && a.CompanyId == companyId));
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Agent agent)
        {
            agent.Configured = true;
            agent.UpdatedIn = DateTime.Now;
            agent.FirstConnection = agent.FirstConnection ?? DateTime.Now;
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();

            return Unit.Successful;
        }

    }
}
