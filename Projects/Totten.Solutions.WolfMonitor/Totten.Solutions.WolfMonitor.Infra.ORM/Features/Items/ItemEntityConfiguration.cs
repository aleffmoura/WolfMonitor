using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items
{
    public class ItemEntityConfiguration : IEntityTypeConfiguration<Item>
    {

        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.UserIdWhoAdd);
            builder.HasIndex(item => item.CompanyId);
            builder.HasIndex(item => item.AgentId);

            builder.Property(item => item.UserIdWhoAdd).IsRequired();
            builder.Property(item => item.CompanyId).IsRequired();
            builder.Property(item => item.AgentId).IsRequired();
            builder.Property(item => item.Name).IsRequired().HasMaxLength(250);
            builder.Property(item => item.Value).IsRequired().HasMaxLength(250);
            builder.Property(item => item.DisplayName).IsRequired().HasMaxLength(250);
            builder.Property(item => item.AboutCurrentValue).IsRequired();
            builder.Property(item => item.CreatedIn).IsRequired();
            builder.Property(item => item.UpdatedIn).IsRequired();
            builder.Property(item => item.Type).IsRequired();

            builder.HasMany(item => item.Historic).WithOne().HasForeignKey(x => x.ItemId);
            builder.HasMany(item => item.SolicitationHistoric).WithOne().HasForeignKey(x => x.ItemId);
        }
    }
}
