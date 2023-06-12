using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kanban.Dashboard.Core.Entities;

namespace Kanban.Dashboard.Infrastructure.EntityConfigurations
{
    public class KanbanTaskConfiguration : IEntityTypeConfiguration<KanbanTask>
    {
        public void Configure(EntityTypeBuilder<KanbanTask> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(4096);
            builder.Property(t => t.Status).IsRequired();

            builder.HasOne(t => t.Column)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Parents)
                .WithMany(x => x.Subtasks)
                .UsingEntity<KanbanTaskSubtask>(
                    l => l.HasOne<KanbanTask>(x=>x.Parent).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(x=>x.ParentId),
                    r => r.HasOne<KanbanTask>(x=>x.Subtask).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(x=>x.SubtaskId));
        }
    }

/*    public class KanbanTaskSubtasksConfiguration : IEntityTypeConfiguration<KanbanTaskSubtask>
    {
        public void Configure(EntityTypeBuilder<KanbanTaskSubtask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Subtask).WithMany(x => x.JoinSubtasks).HasForeignKey(x => x.SubtaskId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Parent).WithMany(x => x.JoinParents).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.NoAction);
        }
    }*/
}
