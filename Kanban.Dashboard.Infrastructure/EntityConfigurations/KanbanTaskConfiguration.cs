using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kanban.Dashboard.Core.Entities;
using Newtonsoft.Json;

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

            builder.ToTable("KanbanTask")
                .HasMany(t => t.Subtasks)
                .WithOne(s => s.KanbanTask)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
