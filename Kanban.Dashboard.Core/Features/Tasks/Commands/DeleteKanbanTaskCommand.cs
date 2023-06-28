using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class DeleteKanbanTaskCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid Id { get; set; }
    }

    public class DeleteKanbanTaskHandler : IRequestHandler<DeleteKanbanTaskCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteKanbanTaskHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteKanbanTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.KanbanTasks.FindAsync(request.Id);
            if (task == null)
                throw new Exception("KanbanTask not found.");

            var column = await _context.Columns.Include(x => x.Board).FirstOrDefaultAsync(x => x.Tasks.Any(y => y.Id == request.Id), cancellationToken);

            if (column != null)
            {
                column.DateOfModification = DateTime.UtcNow;
                column.Board.DateOfModification = DateTime.UtcNow;
            }

            _context.KanbanTasks.Remove(task);
            _context.KanbanTaskSubtask.RemoveRange(_context.KanbanTaskSubtask.Where(x => x.SubtaskId == task.Id || x.ParentId == task.Id));

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
