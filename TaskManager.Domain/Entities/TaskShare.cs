using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public sealed class TaskShare : Entity
{
    public Guid TaskItemId { get; private set; }
    public Guid UserId { get; private set; }
    public SharePermission Permission { get; private set; }
    public DateTime SharedAt { get; private set; }

    private TaskShare() { }

    public TaskShare(Guid taskItemId, Guid userId, SharePermission permission)
    {
        TaskItemId = taskItemId;
        UserId = userId;
        Permission = permission;
        SharedAt = DateTime.UtcNow;
    }

    public void ChangePermission(SharePermission permission)
    {
        Permission = permission;
    }
}
