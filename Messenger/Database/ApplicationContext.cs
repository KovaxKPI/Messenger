using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Role adminRole = new Role { Id = 1, Name = "admin" };
            Role userRole = new Role { Id = 2, Name = "user" };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Nickname = "Tom", Password = "12345", RoleId = 1}
            );
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
