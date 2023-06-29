using Kanban.Dashboard.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Infrastructure.EntityConfigurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(x => x.Users)
                .WithMany(x => x.Boards)
                .UsingEntity<BoardUsers>(
                    x => x.HasOne(y => y.User)
                        .WithMany(y => y.BoardsUsers)
                        .HasForeignKey(y => y.UserId),
                    x => x.HasOne(y => y.Board)
                        .WithMany(y => y.BoardUsers)
                        .HasForeignKey(y => y.BoardId),
                    x => x.HasKey(y => new { y.BoardId, y.UserId }));
        }
    }

}
