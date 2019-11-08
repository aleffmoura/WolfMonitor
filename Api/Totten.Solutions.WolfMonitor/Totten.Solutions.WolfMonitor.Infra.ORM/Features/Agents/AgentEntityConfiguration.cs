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

            builder.HasKey(agent => agent.Id);

            builder.Property(agent => agent.CompanyId).IsRequired();
            builder.Property(agent => agent.UserWhoCreatedId).IsRequired();
            builder.Property(agent => agent.Login).IsRequired().HasMaxLength(100);
            builder.Property(agent => agent.Password).IsRequired().HasMaxLength(100);
        }
    }
}
