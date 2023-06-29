using Kanban.Dashboard.Core;
using Kanban.Dashboard.Core.Entities;
using Kanban.Dashboard.Infrastructure.EntityConfigurations;
using Kanban.Dashboard.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<KanbanTask> KanbanTasks { get; set; }
        public DbSet<KanbanTaskSubtask> KanbanTaskSubtask { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardUsers> BoardUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging();
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BoardConfiguration());
            modelBuilder.ApplyConfiguration(new ColumnConfiguration());
            modelBuilder.ApplyConfiguration(new KanbanTaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BoardUsersEntityTypeConfiguration());

            DataSeeder.IdentitySeed(modelBuilder);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
           // OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
