using TaskManager.Application.DTOs.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.Tasks;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task <TaskItem?> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);

    void Update(TaskItem task);

    void Remove(TaskItem task);
}
