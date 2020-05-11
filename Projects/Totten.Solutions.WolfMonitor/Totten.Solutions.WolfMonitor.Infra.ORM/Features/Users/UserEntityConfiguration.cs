using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.Property(user => user.CompanyId).IsRequired();

            builder.Property(user => user.Login).IsRequired();
            builder.Property(user => user.Password).IsRequired();
            builder.Property(user => user.FirstName).IsRequired();
            builder.Property(user => user.Cpf).IsRequired();
            builder.Property(user => user.Language).IsRequired();
            builder.Property(user => user.CreatedIn).IsRequired();
            builder.Property(user => user.UpdatedIn).IsRequired();
            builder.Property(user => user.Removed).IsRequired();

            builder.Property(user => user.LastLogin);
            builder.Property(user => user.Token);
            builder.Property(user => user.TokenSolicitationCode);
            builder.Property(user => user.RecoverSolicitationCode); ;

            builder.HasIndex(user => user.CompanyId);
            builder.Ignore(user => user.Agents);
            builder.Ignore(role => role.Company);
        }
    }
}
