using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Tasks
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
            var board = await _context.Boards.AnyAsync(x => x.Id == request.BoardId, cancellationToken);
            if (board == false)
                throw new Exception("Board not found.");

            var column = await _context.Columns.AnyAsync(x => x.Id == request.ColumnId, cancellationToken);
            if (column == false)
                throw new Exception("Column not found.");

            var task = await _context.KanbanTasks.FindAsync(request.Id);
            if (task == null) 
                throw new Exception("KanbanTask not found.");

            _context.KanbanTasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
