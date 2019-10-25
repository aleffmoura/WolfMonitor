using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents
{
    public class AgentEntityConfiguration : IEntityTypeConfiguration<Agent>
    {

        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("Agents");

            builder.HasKey(a => a.Id);

            //builder.Property(y => y.Name).IsRequired().HasMaxLength(100);
            //builder.Property(y => y.CustomerName).IsRequired().HasMaxLength(255);
            //builder.Property(y => y.Token).IsRequired().HasMaxLength(100);
            //builder.Property(y => y.Version).IsRequired(false).HasMaxLength(20);
            //builder.Property(y => y.IsRemoved).IsRequired();
            //builder.Property(y => y.Type).IsRequired();
            //builder.Property(y => y.OperationStatus).IsRequired();
            //builder.Property(y => y.Status).IsRequired();
            //builder.Property(y => y.Module).IsRequired().HasMaxLength(50);
            //builder.Property(y => y.LastCommunicationUTC).IsRequired(false).HasConversion(d => d, d => DateTime.SpecifyKind(d.Value, DateTimeKind.Utc));
            //builder.Property(y => y.LastVersionUpdateUTC).IsRequired(false).HasConversion(d => d, d => DateTime.SpecifyKind(d.Value, DateTimeKind.Utc));

            //builder.OwnsOne(x => x.Host).Property(h => h.Name).IsRequired(false).HasColumnName("HostName");
            //builder.OwnsOne(x => x.Host).Property(h => h.IpAddress).IsRequired(false).HasColumnName("HostIpAddress");
            //builder.OwnsOne(x => x.Host).Property(h => h.MacAddress).IsRequired(false).HasMaxLength(50).HasColumnName("HostMacAddress");
            //builder.OwnsOne(x => x.Host).Property(h => h.OS).IsRequired(false).HasMaxLength(255).HasColumnName("HostOS");
            //builder.OwnsOne(x => x.Host).Property(h => h.Type).IsRequired().HasColumnName("HostType");
            
            //builder.HasOne(c => c.LoggingConfig).WithMany().HasForeignKey(c => c.LoggingConfigId);
        }
    }
}
