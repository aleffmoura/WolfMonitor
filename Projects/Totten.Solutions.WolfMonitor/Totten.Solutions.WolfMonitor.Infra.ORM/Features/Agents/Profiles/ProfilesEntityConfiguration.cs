using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents.Profiles
{
    public class ProfilesEntityConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");

            builder.HasKey(profile => profile.Id);
            builder.HasIndex(profile => profile.UserWhoCreatedId);
            builder.HasIndex(profile => profile.CompanyId);
            builder.HasIndex(profile => profile.AgentId);

            builder.Property(profile => profile.ProfileIdentifier).IsRequired();
            builder.Property(profile => profile.CompanyId).IsRequired();
            builder.Property(profile => profile.AgentId).IsRequired();
            builder.Property(profile => profile.Name).IsRequired();
            builder.Property(profile => profile.UserWhoCreatedId).IsRequired();
            builder.Property(profile => profile.CreatedIn).IsRequired();

        }
    }
}
