using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.NoSql.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.NoSql.Features.ItemsMoritoring
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly MonitoringContext _context;

        public ItemsRepository(MonitoringContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, List<Items>>> GetByAgentIdAsync(Guid agentId)
        {
            return await _context.Items.FindAsync(h => h.AgentId == agentId).Result.ToListAsync();
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Items item)
        {
            await _context.Items.ReplaceOneAsync(h => h.AgentId == item.AgentId, item, new UpdateOptions() { IsUpsert = true });

            return Unit.Successful;
        }

        public async Task<Result<Exception, Items>> CreateAsync(Items item)
        {
            var result = Result.Run(() => _context.Items.InsertOne(item));
            var resultFrom = await Task.FromResult(result);

            if (resultFrom.IsSuccess)
                return await GetByIdAsync(item.Id);

            return resultFrom.Failure;
        }
        public async Task<Result<Exception, Unit>> CreateWithUnitReturnAsync(Items item)
        {
            var result = Result.Run(() => _context.Items.InsertOne(item));
            return await Task.FromResult(result);
        }

        public async Task<Result<Exception, Items>> GetByIdAsync(Guid id)
        {
            return await _context.Items.Aggregate()
                .SortByDescending(hm => hm.ReadingDateUTC)
                .Match(GetFilterByAgentId(id.ToString()))
                .FirstOrDefaultAsync();
        }

        public  Result<Exception, IQueryable<Items>> GetAll()
        {
            throw new NotImplementedException();
        }
        private FilterDefinition<Items> GetFilterByAgentId(string agentId)
        {
            return Builders<Items>.Filter.Eq("AgentId", agentId);
        }
    }
}
