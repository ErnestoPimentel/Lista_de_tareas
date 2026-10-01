using System.Data;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Tasks;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(
        ITaskRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskResponse> CreateAsync(
        Guid userId,
        CreateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        var task = new TaskItem(userId, request.Title, request.Description, request.Priority, request.DueDate);

        await _repository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(task);
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(userId, taskId, cancellationToken);

        return task is null ? null : Map(task);
    }

    public async Task<IReadOnlyCollection<TaskResponse>> GetAllAsync(Guid userId,  CancellationToken cancellationToken = default)
    {
        var tasks = await _repository.GetAllAsync(userId, cancellationToken);

        return tasks
            .Select(Map)
            .ToArray();
    }

    public async Task<TaskResponse?> UpdateAsync(Guid userId, Guid taskId, UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(userId, taskId, cancellationToken);

        if (task is null)
            return null;

        task.Update(request.Title, request.Description, request.Priority, request.DueDate);

        await _repository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(task);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(userId, taskId, cancellationToken);

        if(task is null) 
            return false;

        await _repository.Remove(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public static TaskResponse Map(TaskItem task)
    {
        return new TaskResponse(task.Id, task.UserId, task.Title, task.Description, task.Status, task.Priority, task.DueDate, task.CreatedAt, task.UpdatedAt);
    }
}
