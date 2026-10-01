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
    }

    public async Task<TaskItem?> GetByIdAsync(
        Guid userId,
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
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

    public async Task Update(TaskItem task)
    {
        _context.Tasks.Update(task);
    }

    public async Task Remove(TaskItem task)
    {
        _context.Tasks.Remove(task);
    }
}
