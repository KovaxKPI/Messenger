using Microsoft.EntityFrameworkCore;

namespace Messenger.Models
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
    }
}
