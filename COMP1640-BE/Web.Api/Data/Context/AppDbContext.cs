using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;
using Web.Api.Entities;

namespace Web.Api.Data.Context
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IAppDbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions options) : base(options) { }

        // Place DbSet here
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
           

            // Link Many-One || User - Department
            builder.Entity<User>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Idea
            builder.Entity<Idea>()
                .HasOne(x => x.User)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Topic
            builder.Entity<Idea>()
                .HasOne(x => x.Topic)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Category
            builder.Entity<Idea>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || File - Idea
            builder.Entity<File>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - View
            builder.Entity<View>()
                .HasOne(x => x.User)
                .WithMany(x => x.Views)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - View
            builder.Entity<View>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Views)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Comment
            builder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Comment
            builder.Entity<Comment>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.IdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || User - Reaction
            builder.Entity<Reaction>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Many-One || Idea - Reaction
            builder.Entity<Reaction>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.IdeaId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
            
            //Delete "AspNet" name of identity table
            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                var tablename = entityType.GetTableName();
                if(tablename.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tablename.Substring(6));
                }
            }
        }
    }
}
