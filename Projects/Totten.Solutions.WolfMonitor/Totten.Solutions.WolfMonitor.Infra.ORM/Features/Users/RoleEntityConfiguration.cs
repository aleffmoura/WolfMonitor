using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(role => role.Id);

            builder.Property(role => role.Name).IsRequired();
            builder.Property(role => role.Level).IsRequired();
            builder.Property(role => role.CreatedIn).IsRequired();
            builder.Property(role => role.UpdatedIn).IsRequired();
            builder.Property(role => role.Removed).IsRequired();

            builder.HasMany(role => role.Users).WithOne(user => user.Role).HasForeignKey(user => user.RoleId);
        }
    }
}
