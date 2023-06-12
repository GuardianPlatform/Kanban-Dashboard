using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class SwitchTaskColumnCommand : IRequest<bool>
    {
        public Guid TaskId { get; set; }
        public Guid ColumnTargetId { get; set; }
    }

    public class SwitchTaskColumnCommandHandler : IRequestHandler<SwitchTaskColumnCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public SwitchTaskColumnCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SwitchTaskColumnCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.KanbanTasks.Include(x=>x.Column).FirstOrDefaultAsync(x=>x.Id == request.TaskId);
            if (task == null)
                throw new Exception("KanbanTask not found.");


            var column = await _context.Columns.FindAsync(request.ColumnTargetId, cancellationToken);
            if (column == null || task.Column.BoardId != column?.BoardId)
                throw new Exception("Target column is located in different board than source column");

            task.ColumnId = request.ColumnTargetId;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
