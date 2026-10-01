using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.Tasks;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task <TaskItem?> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);

    Task Update(TaskItem task, CancellationToken cancellationToken = default);

    Task Remove(TaskItem task, CancellationToken cancellationToken = default);
}
