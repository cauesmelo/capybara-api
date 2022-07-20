using capybara_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace capybara_api.Infra;

public class AppDbContext : IdentityDbContext<IdentityUser> {
    public DbSet<Reminder> reminder { get; set; }
    public DbSet<TaskList> taskList { get; set; }
    public DbSet<TaskUnity> taskUnity { get; set; }
    public DbSet<Note> note { get; set; }
    public DbSet<Preference> preference { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<Reminder>()
            .Property(p => p.title).HasMaxLength(255);
        builder.Entity<TaskList>()
            .Property(p => p.title).HasMaxLength(255);
        builder.Entity<TaskUnity>()
            .Property(p => p.title).HasMaxLength(255);
        builder.Entity<Note>()
            .Property(p => p.content).HasMaxLength(255);
    }

    public override int SaveChanges() {
        AddTimestamps();
        return base.SaveChanges();
    }

    private void AddTimestamps() {
        IEnumerable<EntityEntry> entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach(EntityEntry entity in entities) {
            DateTime now = DateTime.UtcNow;

            if(entity.State == EntityState.Added) {
                ((BaseModel) entity.Entity).createdAt = now;
            }
            ((BaseModel) entity.Entity).updatedAt = now;
        }
    }
}
