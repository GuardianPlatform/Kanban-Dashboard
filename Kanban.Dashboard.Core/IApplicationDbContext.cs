using Kanban.Dashboard.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core
{
    public interface IApplicationDbContext
    {
        DbSet<Board> Boards { get; set; }
        DbSet<Column> Columns { get; set; }
        DbSet<KanbanTask> KanbanTasks { get; set; }
        DbSet<KanbanTaskSubtask> KanbanTaskSubtask { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}