using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class MoveTaskCommand : IRequest<bool>
    {
        public Guid TaskId { get; set; }
        public Guid ColumnTargetId { get; set; }
        public int Order { get; set; }
    }

    public class MoveTaskColumnCommandHandler : IRequestHandler<MoveTaskCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public MoveTaskColumnCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
        {
            var switchColumnResult = await _mediator.Send(new SwitchTaskColumnCommand() { ColumnTargetId = request.ColumnTargetId, TaskId = request.TaskId }, cancellationToken);
            if (switchColumnResult == false)
                throw new Exception("Switching column failed.");

            var column = await _context.Columns.FirstOrDefaultAsync(x => x.Id == request.ColumnTargetId, cancellationToken);
            if (column == null)
                throw new Exception("Target column does not exist.");

            var tasks = _context.KanbanTasks.Where(x => x.ColumnId == request.ColumnTargetId);
            var task = await tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken: cancellationToken);
            if (task == null)
                throw new Exception("KanbanTask not found.");

            var order = Math.Max(request.Order, 0);
            var tasksWithOrderGreaterOrEqual = await tasks.Where(x => x.Order >= order).ToListAsync(cancellationToken);
            foreach (var taskWithOrderGreaterOrEqual in tasksWithOrderGreaterOrEqual)
            {
                taskWithOrderGreaterOrEqual.Order++;
            }
            task.Order = order;

            var allTasksInColumn = (await tasks.ToListAsync(cancellationToken)).OrderBy(x => x.Order);
            var normalizedOrder = 1;
            foreach (var t in allTasksInColumn)
            {
                t.Order = normalizedOrder++;
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
