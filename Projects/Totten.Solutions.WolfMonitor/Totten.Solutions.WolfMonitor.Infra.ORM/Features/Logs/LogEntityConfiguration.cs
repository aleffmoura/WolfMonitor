using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Logs
{
    public class LogEntityConfiguration : IEntityTypeConfiguration<Log>
    {

        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(log => log.Id);
            builder.Property(role => role.UserId).IsRequired();
            builder.Property(role => role.UserCompanyId).IsRequired();
            builder.Property(role => role.TargetId).IsRequired();
            builder.Property(role => role.EntityType).IsRequired();
            builder.Property(role => role.TypeLogMethod).IsRequired();
            builder.Property(role => role.TargetId).IsRequired();
            builder.Property(role => role.OldValue);
            builder.Property(role => role.NewValue);
            builder.Property(role => role.CreatedIn).IsRequired();
            builder.Property(role => role.UpdatedIn).IsRequired();
        }
    }
}
