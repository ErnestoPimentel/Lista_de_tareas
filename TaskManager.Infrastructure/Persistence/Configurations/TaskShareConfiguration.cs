using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations;

public sealed class TaskShareConfiguration : IEntityTypeConfiguration<TaskShare>
{
    public void Configure(EntityTypeBuilder<TaskShare> builder)
    {
        builder.ToTable("TaskShares");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Permission)
            .IsRequired();

        builder.Property(x => x.SharedAt)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.TaskItemId,
            x.UserId
        }).IsUnique();

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
