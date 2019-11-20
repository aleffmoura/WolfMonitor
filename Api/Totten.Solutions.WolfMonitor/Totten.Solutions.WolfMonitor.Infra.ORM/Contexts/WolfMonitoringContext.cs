using Microsoft.EntityFrameworkCore;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.SystemServices;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class WolfMonitoringContext : DbContext
    {
        public DbSet<SystemService> SystemServices { get; set; }

        public WolfMonitoringContext(DbContextOptions<WolfMonitoringContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SystemServiceEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
    }
}
