using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class DesignTimeWolfMonitoringContextFactory : IDesignTimeDbContextFactory<WolfMonitoringContext>
    {
        public WolfMonitoringContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<WolfMonitoringContext> optionsBuilder = new DbContextOptionsBuilder<WolfMonitoringContext>();

            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WolfMonitoringContext;Persist Security Info=True; Integrated Security=True;",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new WolfMonitoringContext(optionsBuilder.Options);
        }
    }
}
