using Microsoft.EntityFrameworkCore;
using Web.Api.Entities;

namespace Web.Api.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions options) : base(options) { }

        // Place DbSet here
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        // Entity Linking
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Link Many-One || User - Role
            builder.Entity<User>()
                .HasOne<Role>(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Department
            builder.Entity<User>()
                .HasOne<Department>(x => x.Department)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Idea
            builder.Entity<Idea>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Topic
            builder.Entity<Idea>()
                .HasOne<Topic>(x => x.Topic)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Category
            builder.Entity<Idea>()
                .HasOne<Category>(x => x.Category)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || File - Idea
            builder.Entity<File>()
                .HasOne<Idea>(x => x.Idea)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - View
            builder.Entity<View>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Views)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - View
            builder.Entity<View>()
                .HasOne<Idea>(x => x.Idea)
                .WithMany(x => x.Views)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Comment
            builder.Entity<Comment>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Comment
            builder.Entity<Comment>()
                .HasOne<Idea>(x => x.Idea)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Reaction
            builder.Entity<Reaction>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Reaction
            builder.Entity<Reaction>()
                .HasOne<Idea>(x => x.Idea)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
