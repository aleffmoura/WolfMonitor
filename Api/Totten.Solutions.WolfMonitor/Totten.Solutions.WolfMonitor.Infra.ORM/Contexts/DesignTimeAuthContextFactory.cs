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
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AuthContext;Persist Security Info=True; Integrated Security=True;",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new AuthContext(optionsBuilder.Options);
        }
    }
}
