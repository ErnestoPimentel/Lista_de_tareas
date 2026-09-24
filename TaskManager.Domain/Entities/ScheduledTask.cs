using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities;

public sealed class ScheduledTask : Entity
{
    public Guid TaskItemId { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public bool IsExecuted { get; private set; }

    private ScheduledTask() { }

    public ScheduledTask(Guid taskItemId, DateTime scheduledAt)
    {
        TaskItemId = taskItemId;
        ScheduledAt = scheduledAt;
    }

    public void MarkAsExecuted()
    {
        IsExecuted = true;
    }

    public void Reschedule(DateTime scheduledAt)
    {
        ScheduledAt = scheduledAt;
        IsExecuted = false;
    }
}
