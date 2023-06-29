using Kanban.Dashboard.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Infrastructure.EntityConfigurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();
    }
}