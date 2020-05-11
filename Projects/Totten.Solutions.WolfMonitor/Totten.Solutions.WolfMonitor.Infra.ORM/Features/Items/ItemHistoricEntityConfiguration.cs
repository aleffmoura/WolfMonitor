using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items
{
    public class ItemHistoricEntityConfiguration : IEntityTypeConfiguration<ItemHistoric>
    {

        public void Configure(EntityTypeBuilder<ItemHistoric> builder)
        {
            builder.ToTable("Historic");

            builder.HasKey(item => item.Id);

            builder.Property(item => item.Value).IsRequired().HasMaxLength(250);
            builder.Property(item => item.CreatedIn).IsRequired();
            builder.Property(item => item.UpdatedIn).IsRequired();
        }
    }
}
