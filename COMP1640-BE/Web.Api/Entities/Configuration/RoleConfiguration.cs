using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Api.Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Name = "QA Manager",
                NormalizedName = "QA MANAGER"
            },
            new IdentityRole
            {
                Name = "QA Coordinator",
                NormalizedName = "QA COORDINATOR"
            },
            new IdentityRole
            {
                Name = "Staff",
                NormalizedName = "STAFF"
            }
            );
        }
    }
}
