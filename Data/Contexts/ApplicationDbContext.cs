using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUserEntity>(options)

{
    public virtual DbSet<ProjectEntity> Projects { get; set; }
    public virtual DbSet<StatusEntity> Statuses { get; set; }



    //Delar av koden är genererad av ChatGPT
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        // Configure default values or seed data for StatusEntity
        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { Id = 1, StatusName = "Started" },
            new StatusEntity { Id = 2, StatusName = "Completed" }
        );

        // Configure ProjectEntity relationships more explicitly if needed
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.StatusId);
    }
}
