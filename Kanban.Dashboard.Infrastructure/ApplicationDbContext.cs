using Kanban.Dashboard.Core;
using Kanban.Dashboard.Core.Entities;
using Kanban.Dashboard.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kanban.Dashboard.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<KanbanTask> KanbanTasks { get; set; }
        public DbSet<KanbanTaskSubtask> KanbanTaskSubtask { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
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
            // modelBuilder.ApplyConfiguration(new KanbanTaskSubtasksConfiguration());
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
           // OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private void OnBeforeSaving()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                entry.Entity.DateOfModification = now;

                if (entry.State == EntityState.Added)
                    entry.Entity.DateOfCreation = now;
            }
        }
    }
}
