using Microsoft.EntityFrameworkCore;
using Helpdesk.Entities;

namespace Helpdesk.DataAccess
{
  public class PostgresDbContext : DbContext
  {
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
      : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Department>(entity =>
      {
        entity.HasKey(department => department.Id);
        entity.Property(department => department.DepartmentName).IsRequired();

        entity.HasMany(department => department.Users)
          .WithOne(user => user.Department)
          .HasForeignKey(user => user.DepartmentId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(department => department.SupportRequests)
          .WithOne(request => request.AssignedDepartment)
          .HasForeignKey(request => request.AssignedDepartmentId)
          .OnDelete(DeleteBehavior.SetNull);
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(user => user.Id);
        entity.Property(user => user.Email).IsRequired();
        entity.Property(user => user.Password).IsRequired();
        entity.HasIndex(user => user.Email).IsUnique();
      });

      modelBuilder.Entity<SupportRequest>(entity =>
      {
        entity.HasKey(request => request.Id);
        entity.Property(request => request.RequestCode).IsRequired();
        entity.Property(request => request.Status).HasConversion<int>();
        entity.HasIndex(request => request.RequestCode).IsUnique();
      });
    }
  }
}
