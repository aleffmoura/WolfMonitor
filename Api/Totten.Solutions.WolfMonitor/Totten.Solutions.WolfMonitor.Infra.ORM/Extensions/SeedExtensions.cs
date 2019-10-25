using Microsoft.EntityFrameworkCore;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Extensions
{
    public static class SeedExtensions
    {
        public static void SeedAuth(this ModelBuilder builder)
        {
            //string password = "123";
            //Guid id1 = Guid.NewGuid();

            //builder.Entity<Role>().HasData(new Role()
            //{
            //    Id = id1,
            //    Name = "Admin"
            //});

            //builder.Entity<User>().HasData(new User()
            //{
            //    Id = Guid.NewGuid(),
            //    Username = "admin",
            //    Email = "admin@admin.com",
            //    Cpf = "11122233344",
            //    Password = password.GenerateHash(),
            //    RoleId = id1
            //});
        }
    }
}
