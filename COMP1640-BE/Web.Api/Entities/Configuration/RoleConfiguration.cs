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
                IdentityRoles.Administrator_Identity,
                IdentityRoles.QAManager_Identity,
                IdentityRoles.QACoordinator_Identity,
                IdentityRoles.Staff_Identity
                );
        }
    }
}
