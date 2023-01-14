using Microsoft.EntityFrameworkCore;

namespace Web.Api.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        // Place DbSet here


        // Entity Linking
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
