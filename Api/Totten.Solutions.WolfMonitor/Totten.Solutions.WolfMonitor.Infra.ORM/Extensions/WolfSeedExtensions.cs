using Microsoft.EntityFrameworkCore;
using System;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Extensions
{
    public static class WolfSeedExtensions
    {
        public static void SeedWolf(this ModelBuilder builder)
        {
            Guid companyId = Guid.Parse("c576cf93-370c-4464-21f9-08d763d27d75");

            #region Create User
            builder.Entity<Company>().HasData(new Company
            {
                Id = companyId,
                Name = "ALEFF MOURA DA SILVA 10685805425",
                FantasyName = "TOTEM SOLUTIONS",
                Cnpj = "35.344.681/0001-90",
                CreatedIn = DateTime.Now,
                Removed = false,
                UpdatedIn = DateTime.Now
            });
            #endregion
        }
    }
}
