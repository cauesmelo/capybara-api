using capybara_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace capybara_api.Infra
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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

            base.OnModelCreating(builder);

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
