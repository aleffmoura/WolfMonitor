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

            builder.HasKey(systemService => systemService.Id);
            builder.HasIndex(systemService => systemService.UserIdWhoAdd);
            builder.HasIndex(systemService => systemService.AgentId);

            builder.Property(systemService => systemService.UserIdWhoAdd).IsRequired();
            builder.Property(systemService => systemService.AgentId).IsRequired();
            builder.Property(systemService => systemService.Name).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.Value).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.DisplayName).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.Interval).IsRequired();
            builder.Property(systemService => systemService.CreatedIn).IsRequired();
            builder.Property(systemService => systemService.UpdatedIn).IsRequired();
            builder.Property(systemService => systemService.Type).IsRequired();
        }
    }
}
