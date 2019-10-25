using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class DesignTimeAuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AuthContext> optionsBuilder = new DbContextOptionsBuilder<AuthContext>();

            optionsBuilder.UseNpgsql("Host=10.0.75.1;Port=5434;Database=auth;Username=postgres;Password=Ud5#B4tP4Y!",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new AuthContext(optionsBuilder.Options);
        }
    }
}
