using Microsoft.EntityFrameworkCore;
using Kanban.Dashboard.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kanban.Dashboard.Infrastructure.EntityConfigurations
{
    public class ColumnConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            builder.HasOne(c => c.Board)
                .WithMany(b => b.Columns)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
