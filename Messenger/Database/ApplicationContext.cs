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
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Nickname = "Tom", Password = "12345" }
            );
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

    }
}
