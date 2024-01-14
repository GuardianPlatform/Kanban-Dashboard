using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class SwitchTaskColumnCommand : IRequest<(bool isSuccess, Guid oldColumn)>
    {
        public Guid TaskId { get; set; }
        public Guid ColumnTargetId { get; set; }
    }

    public class SwitchTaskColumnCommandHandler : IRequestHandler<SwitchTaskColumnCommand, (bool isSuccess, Guid oldColumn)>
    {
        private readonly IApplicationDbContext _context;

        public SwitchTaskColumnCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool isSuccess, Guid oldColumn)> Handle(SwitchTaskColumnCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.KanbanTasks.Include(x => x.Column).FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken: cancellationToken);
            if (task == null)
                throw new Exception("KanbanTask not found.");

            var oldColumnId = task.ColumnId;

            var column = await _context.Columns.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == request.ColumnTargetId, cancellationToken);
            if (column == null || task.Column.BoardId != column?.BoardId)
                throw new Exception("Target column is located in different board than source column or target column does not exist.");

            if (request.ColumnTargetId == task.ColumnId)
                return (false, Guid.Empty);

            task.DateOfModification = DateTime.UtcNow;
            task.Column.DateOfModification = DateTime.UtcNow;
            column.DateOfModification = DateTime.UtcNow;
            column.Board.DateOfModification = DateTime.UtcNow;
            task.ColumnId = request.ColumnTargetId;
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return (result, oldColumnId);
        }
    }
}
