using Microsoft.EntityFrameworkCore;
using starter.Application.Common.Interfaces;
using starter.Domain.Entities;

namespace starter.Infrastructure.Data;

public class StarterDbContext : DbContext, IApplicationDbContext
{
    public StarterDbContext(DbContextOptions<StarterDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.MiddleName)
                .HasMaxLength(100);

            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });
    }
}