using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _context;

    public UnitOfWork(TaskManagerDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
