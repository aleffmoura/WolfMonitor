using Microsoft.EntityFrameworkCore;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class WolfMonitorContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Company> Companies { get; set; }

        public WolfMonitorContext(DbContextOptions<WolfMonitorContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AgentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());

            //modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
    }
}
