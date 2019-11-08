using Microsoft.EntityFrameworkCore;
using System;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Extensions
{
    public static class SeedExtensions
    {
        public static void SeedAuth(this ModelBuilder builder)
        {
            Guid idSystem = Guid.NewGuid();

            #region Create Roles
            builder.Entity<Role>().HasData(new Role()
            {
                Name = "Agent",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.Agent,
                Removed = false,
                Id = Guid.NewGuid()
            });
            builder.Entity<Role>().HasData(new Role()
            {
                Name = "User",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.User,
                Removed = false,
                Id = Guid.NewGuid()
            });
            builder.Entity<Role>().HasData(new Role()
            {
                Name = "Administrador",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.Admin,
                Removed = false,
                Id = Guid.NewGuid()
            });
            builder.Entity<Role>().HasData(new Role()
            {
                Name = "System",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.Admin,
                Removed = false,
                Id = idSystem
            });
            #endregion

            #region Create User
            builder.Entity<User>().HasData(new User()
            {
                Id = Guid.NewGuid(),
                Login = "alefmoura",
                Email = "aleffmds@gmail.com",
                Cpf = "10685805425",
                Password = "88633251".GenerateHash(),
                RoleId = idSystem,
                FirstName = "Aleff",
                LastName = "Moura da Silva",
                Language = "pt-BR",
                CreatedIn = DateTime.Now,
                Removed = false,
                UpdatedIn = DateTime.Now,
                CompanyId = Guid.Parse("c576cf93-370c-4464-21f9-08d763d27d75")
            });
            #endregion
        }
    }
}