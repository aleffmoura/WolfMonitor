using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents
{
    public class AgentEntityConfiguration : IEntityTypeConfiguration<Agent>
    {

        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("Agents");

            builder.HasKey(agent => agent.Id);
            builder.HasIndex(agent => agent.UserWhoCreatedId);

            builder.Property(agent => agent.DisplayName).IsRequired();
            builder.Property(agent => agent.UserWhoCreatedId).IsRequired();
            builder.Property(agent => agent.ReadItemsMonitoringByArchive).IsRequired();
            builder.Property(agent => agent.Login).IsRequired().HasMaxLength(100);
            builder.Property(agent => agent.Password).IsRequired().HasMaxLength(100);
            builder.Property(agent => agent.CreatedIn).IsRequired();
            builder.Property(agent => agent.ProfileIdentifier);
            builder.Property(agent => agent.ProfileName);

            builder.Ignore(agent => agent.UserWhoCreated);
            builder.Ignore(agent => agent.Items);
        }
    }
}
