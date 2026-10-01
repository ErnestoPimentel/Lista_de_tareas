using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public sealed record CreateTaskRequest(string Title, string? Description, TaskPriority Priority, DateTime? DueDate);
