using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations;

public sealed class ScheduledTaskConfiguration : IEntityTypeConfiguration<ScheduledTask>
{
    public void Configure(EntityTypeBuilder<ScheduledTask> builder)
    {
        builder.ToTable("ScheduledTasks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ScheduledAt)
            .IsRequired();

        builder.Property(x => x.IsExecuted)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.ScheduledAt,
            x.IsExecuted
        });

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}