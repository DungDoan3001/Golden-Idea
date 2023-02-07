using Microsoft.EntityFrameworkCore;
using Web.Api.Entities;

namespace Web.Api.Data
{
    public interface IAppDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Idea> Ideas { get; set; }
        DbSet<Reaction> Reactions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<View> Views { get; set; }
    }
}