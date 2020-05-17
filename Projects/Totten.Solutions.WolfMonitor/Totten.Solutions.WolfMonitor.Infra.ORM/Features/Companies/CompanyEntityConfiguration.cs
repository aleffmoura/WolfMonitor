using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Companies
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
    {

        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(company => company.Id);
            builder.Property(company => company.Name).IsRequired();
            builder.Property(company => company.FantasyName).IsRequired();
            builder.Property(company => company.Cnpj).IsRequired();
            builder.Property(company => company.Address).IsRequired();
            builder.Property(company => company.Email).IsRequired();
            builder.Property(company => company.Phone).IsRequired();

            builder.HasMany(company => company.Agents).WithOne(agent => agent.Company).HasForeignKey(company => company.CompanyId);
            builder.Ignore(company => company.Users);
        }
    }
}
