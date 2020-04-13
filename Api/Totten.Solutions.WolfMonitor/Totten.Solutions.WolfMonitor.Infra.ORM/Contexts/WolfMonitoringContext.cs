using Microsoft.EntityFrameworkCore;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class WolfMonitoringContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemHistoric> Historic { get; set; }
        public DbSet<ItemSolicitationHistoric> SolicitationsHistoric { get; set; }

        public WolfMonitoringContext(DbContextOptions<WolfMonitoringContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ItemHistoricEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ItemSolicitationHistoricEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
    }
}
