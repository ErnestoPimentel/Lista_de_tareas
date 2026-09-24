using System.Security.Cryptography.X509Certificates;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public sealed class TaskItem
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public Enums.TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TaskItem() { }

    public TaskItem(Guid userGuid, string title, string? description, TaskPriority priority, DateTime? dueDate)
    {
        UserId = userGuid;
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        Status = Enums.TaskStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string? description, TaskPriority priority, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(Enums.TaskStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
