using Microsoft.EntityFrameworkCore;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Logs;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class LogContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public LogContext(DbContextOptions<LogContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
