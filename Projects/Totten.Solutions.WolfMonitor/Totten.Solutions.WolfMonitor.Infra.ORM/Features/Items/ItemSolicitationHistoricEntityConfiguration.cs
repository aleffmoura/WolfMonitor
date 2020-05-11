using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items
{
    public class ItemSolicitationHistoricEntityConfiguration : IEntityTypeConfiguration<ItemSolicitationHistoric>
    {

        public void Configure(EntityTypeBuilder<ItemSolicitationHistoric> builder)
        {
            builder.ToTable("SolicitationsHistoric");

            builder.HasKey(item => item.Id);
            builder.HasIndex(item => item.UserId);
            builder.HasIndex(item => item.AgentId);
            builder.HasIndex(item => item.CompanyId);
            builder.HasIndex(item => item.ItemId);

            builder.Property(item => item.UserId).IsRequired();
            builder.Property(item => item.AgentId).IsRequired();
            builder.Property(item => item.CompanyId).IsRequired();
            builder.Property(item => item.ItemId).IsRequired();
            builder.Property(item => item.Name).IsRequired().HasMaxLength(250);
            builder.Property(item => item.DisplayName).IsRequired().HasMaxLength(250);
            builder.Property(item => item.NewValue).IsRequired().HasMaxLength(250);
            builder.Property(item => item.CreatedIn).IsRequired();
            builder.Property(item => item.UpdatedIn).IsRequired();
            builder.Property(item => item.SolicitationType).IsRequired();

            builder.Ignore(item => item.User);
        }
    }
}
