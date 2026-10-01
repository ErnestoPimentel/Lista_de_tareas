using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Application.Interfaces.Tasks;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories.Tasks;

public sealed class TaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _context;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == taskId &&
                x.UserId == userId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(
                x => x.UserId == userId)
            .OrderByDescending(
                x => x.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    public async Task Update(TaskItem task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(TaskItem task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
