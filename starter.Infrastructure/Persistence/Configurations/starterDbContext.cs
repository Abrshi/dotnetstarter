using Microsoft.EntityFrameworkCore;
using starter.Domain.Entities;

namespace starterBackend.Infrastructure.Data;

public class starterBackendDbContext (DbContextOptions<starterBackendDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(starterBackendDbContext).Assembly);
    }
}