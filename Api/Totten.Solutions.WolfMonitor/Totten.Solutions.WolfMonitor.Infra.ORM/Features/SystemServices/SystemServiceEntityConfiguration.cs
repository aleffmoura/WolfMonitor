using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.SystemServices
{
    public class SystemServiceEntityConfiguration : IEntityTypeConfiguration<SystemService>
    {

        public void Configure(EntityTypeBuilder<SystemService> builder)
        {
            builder.ToTable("SystemServices");

            builder.HasKey(systemService => systemService.Id);
            builder.HasIndex(systemService => systemService.UserIdWhoAdd);
            builder.HasIndex(systemService => systemService.AgentId);

            builder.Property(systemService => systemService.UserIdWhoAdd).IsRequired();
            builder.Property(systemService => systemService.AgentId).IsRequired();
            builder.Property(systemService => systemService.Name).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.Value).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.DisplayName).IsRequired().HasMaxLength(250);
            builder.Property(systemService => systemService.CreatedIn).IsRequired();
            builder.Property(systemService => systemService.UpdatedIn).IsRequired();
        }
    }
}
