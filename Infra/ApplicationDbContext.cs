using capybara_api.Models;
using Microsoft.EntityFrameworkCore;

namespace capybara_api.Infra
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Preference> Preference { get; set; }
        public DbSet<Reminder> Reminder { get; set; }
        public DbSet<TaskList> TaskList { get; set; }
        public DbSet<TaskUnity> TaskUnity { get; set; }
        public DbSet<Note> Note { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // builder.Ignore<Notification>();

            builder.Entity<Reminder>()
                .Property(p => p.Title).HasMaxLength(255);
            builder.Entity<TaskList>()
                .Property(p => p.Title).HasMaxLength(255);
            builder.Entity<TaskUnity>()
                .Property(p => p.Title).HasMaxLength(255);
            builder.Entity<Note>()
                .Property(p => p.Title).HasMaxLength(255);
        }
    }
}
