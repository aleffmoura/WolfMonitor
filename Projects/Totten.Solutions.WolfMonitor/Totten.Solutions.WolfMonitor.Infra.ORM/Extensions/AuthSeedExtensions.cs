using Microsoft.EntityFrameworkCore;
using System;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Extensions
{
    public static class AuthSeedExtensions
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
                Name = "Admin",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.Admin,
                Removed = false,
                Id = Guid.Parse("f91a2366-c469-412a-9197-976a90516272")
            });
            builder.Entity<Role>().HasData(new Role()
            {
                Name = "System",
                CreatedIn = DateTime.Now,
                UpdatedIn = DateTime.Now,
                Level = RoleLevelEnum.System,
                Removed = false,
                Id = idSystem
            });
            #endregion

            #region Create User
            builder.Entity<User>().HasData(new User()
            {
                Id = Guid.Parse("f75a1881-0fd6-4273-9d23-c59018788201"),
                Login = "aleffmoura",
                Email = "aleffmds@gmail.com",
                Cpf = "11111111111",
                Password = "123456".GenerateHash(),
                RoleId = idSystem,
                FirstName = "Aleff",
                LastName = "Moura da Silva",
                Language = "pt-BR",
                CreatedIn = DateTime.Now,
                Removed = false,
                LastLogin = null,
                UpdatedIn = DateTime.Now,
                CompanyId = Guid.Parse("c576cf93-370c-4464-21f9-08d763d27d75")
            });
            #endregion
        }
    }
}