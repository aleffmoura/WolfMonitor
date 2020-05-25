using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Logs
{
    public class LogRepository : ILogRepository
    {
        private readonly LogContext _context;

        public LogRepository(LogContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, Log>> CreateAsync(Log entity)
        {
            entity.Validate();

            var newEntity = _context.Logs.Add(entity);

            await _context.SaveChangesAsync();

            return newEntity.Entity;
        }

        public Result<Exception, IQueryable<Log>> GetAll()
                 => Result.Run(() => _context.Logs.Where(a => !a.Removed));

        public async Task<Result<Exception, Log>> GetByIdAsync(Guid id)
        {
            Log user = await _context.Logs.FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (user == null)
                return new NotFoundException();

            return user;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Log entity)
        {
            _context.Logs.Update(entity);

            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
