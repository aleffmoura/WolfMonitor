using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.ORM.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class AuthContext : DbContext
    {
        public DbSet<User> Users;
        public DbSet<Role> Roles;

        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());

            modelBuilder.SeedAuth();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
    }
}
