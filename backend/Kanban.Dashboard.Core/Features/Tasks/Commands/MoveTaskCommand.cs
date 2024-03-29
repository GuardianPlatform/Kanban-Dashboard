﻿using MediatR;
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

            var column = await _context.Columns.FirstOrDefaultAsync(x => x.Id == request.ColumnTargetId, cancellationToken);
            if (column == null)
                throw new Exception("Target column does not exist.");

            var tasks = _context.KanbanTasks.Where(x => x.ColumnId == request.ColumnTargetId);
            var task = await tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken: cancellationToken);
            if (task == null)
                throw new Exception("KanbanTask not found.");
           
            var order = Math.Max(request.Order, 0);

            if (switchColumnResult.isSuccess)
            {
                foreach (var taskWithOrderGreaterOrEqual in tasks.Where(x => x.Order <= order - 1 && x.Id != task.Id && task.Parents.Any() == false))
                {
                    taskWithOrderGreaterOrEqual.Order--;
                }

                foreach (var taskWithOrderGreaterOrEqual in tasks.Where(x => x.Order >= order && x.Id != task.Id && task.Parents.Any() == false))
                {
                    taskWithOrderGreaterOrEqual.Order++;
                }

                task.Order = order;

                var i = 1;
                foreach (var t in (await tasks.Where(x => x.Parents.Any() == false).ToListAsync(cancellationToken)).OrderBy(x => x.Order))
                {
                    t.Order = i++;
                }
                await tasks.Where(x => x.Parents.Any() == true).ForEachAsync(x => x.Order = 0, cancellationToken);

                var oldColumn = _context.KanbanTasks.Where(x => x.ColumnId == switchColumnResult.oldColumn);
                i = 1;
                foreach (var t in (await oldColumn.Where(x => x.Parents.Any() == false).ToListAsync(cancellationToken)).OrderBy(x => x.Order))
                {
                    t.Order = i++;
                }
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }

            var tasksWithOrderLessOrEqual = order > (switchColumnResult.isSuccess ? 0 : task.Order)
                ? tasks.Where(x => x.Order <= order && x.Id != task.Id && task.Parents.Any() == false)
                : tasks.Where(x => x.Order < order - 1 && x.Id != task.Id && task.Parents.Any() == false);

            foreach (var taskWithOrderGreaterOrEqual in tasksWithOrderLessOrEqual)
            {
                taskWithOrderGreaterOrEqual.Order--;
            }

            var tasksWithOrderGreater = order > (switchColumnResult.isSuccess ? 0 : task.Order)
                ? await tasks.Where(x => x.Order > order && x.Id != task.Id && task.Parents.Any() == false).ToListAsync(cancellationToken)
                : await tasks.Where(x => x.Order >= order && x.Id != task.Id && task.Parents.Any() == false).ToListAsync(cancellationToken);

            foreach (var taskWithOrderGreaterOrEqual in tasksWithOrderGreater)
            {
                taskWithOrderGreaterOrEqual.Order++;
            }
            task.Order = order;

            var allTasksInColumn = (await tasks.Where(x => x.Parents.Any() == false).ToListAsync(cancellationToken)).OrderBy(x => x.Order);
            var normalizedOrder = 1;
            foreach (var t in allTasksInColumn)
            {
                t.Order = normalizedOrder++;
            }

            await tasks.Where(x => x.Parents.Any() == true).ForEachAsync(x => x.Order = 0, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
