using TaskManager.Application.DTOs.Tasks;

namespace TaskManager.Application.Interfaces.Tasks;

public interface ITaskService
{
    Task<TaskResponse> CreateAsync(Guid userId, CreateTaskRequest request, CancellationToken cancellationToken = default);

    Task<TaskResponse?> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskResponse>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<TaskResponse?> UpdateAsync(Guid userId, Guid taskId, UpdateTaskRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid userId, Guid taskId, CancellationToken cancellationToken= default);
}
