using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public sealed record TaskResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string? Description,
    Domain.Enums.TaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? UpdateAt);