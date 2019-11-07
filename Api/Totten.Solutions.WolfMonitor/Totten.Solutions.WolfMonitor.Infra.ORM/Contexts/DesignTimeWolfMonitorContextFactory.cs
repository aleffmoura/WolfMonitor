using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class DesignTimeWolfMonitorContextFactory : IDesignTimeDbContextFactory<WolfMonitorContext>
    {
        public WolfMonitorContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<WolfMonitorContext> optionsBuilder = new DbContextOptionsBuilder<WolfMonitorContext>();

            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=WolfMonitorContext;Persist Security Info=True;User ID=sa;Password=uds@00",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new WolfMonitorContext(optionsBuilder.Options);
        }
    }
}
