using Kanban.Dashboard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kanban.Dashboard.Infrastructure.EntityConfigurations
{
    public class BoardUsersEntityTypeConfiguration : IEntityTypeConfiguration<BoardUsers>
    {
        public void Configure(EntityTypeBuilder<BoardUsers> builder)
        {
            builder.HasKey(x => new { x.BoardId, x.UserId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.BoardsUsers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Board)
                .WithMany(x => x.BoardUsers)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
