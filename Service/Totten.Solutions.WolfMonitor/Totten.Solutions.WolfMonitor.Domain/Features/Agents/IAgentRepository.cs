using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public interface IAgentRepository
    {
        public Task<Guid> AddAsync(Agent agent);
        public Task<Agent> GetById(Guid id);
    }
}
