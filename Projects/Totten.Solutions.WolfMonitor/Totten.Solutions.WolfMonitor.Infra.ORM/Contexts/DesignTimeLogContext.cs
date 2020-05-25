using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class DesignTimeLogContext : IDesignTimeDbContextFactory<LogContext>
    {
        public LogContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<LogContext> optionsBuilder = new DbContextOptionsBuilder<LogContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogContext;Persist Security Info=True; Integrated Security=True;",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new LogContext(optionsBuilder.Options);
        }
    }
}

