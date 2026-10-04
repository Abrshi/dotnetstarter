using Microsoft.EntityFrameworkCore;
using starter.Domain.Entities;

namespace starter.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

