﻿using Microsoft.EntityFrameworkCore;
using System;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Extensions
{
    public static class WolfSeedExtensions
    {
        public static void SeedWolf(this ModelBuilder builder)
        {
            Guid companyId = Guid.Parse("c576cf93-370c-4464-21f9-08d763d27d75");

            #region Create User
            var company = new Company
            {
                Id = companyId,
                Name = "ALEFF MOURA DA SILVA",
                FantasyName = "tottemsolutions",
                Cnpj = "35.344.681/0001-90",
                Address = "Rua Cicero Lourenço, Mossoró/RN",
                Phone = "(49) 9 9914-6350",
                Email = "aleffmds@gmail.com",
                Cnae = "",
                MunicipalRegistration = "",
                StateRegistration = "",
                CreatedIn = DateTime.Now,
                Removed = false,
                UpdatedIn = DateTime.Now
            };
            builder.Entity<Company>().HasData(company);
            builder.Entity<Agent>().HasData(new Agent
            {
                CompanyId = companyId,
                CreatedIn = DateTime.Now,
                DisplayName = "Servidor BR 1",
                Id = Guid.NewGuid(),
                Login = "servidor1",
                Password = "123456".GenerateHash(),
                UserWhoCreatedId = Guid.Parse("f91a2366-c469-412a-9197-976a90516272"),
                UserWhoCreatedName = "Admin"

            });
            #endregion
        }
    }
}
