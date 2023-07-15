using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class ReorderTasksCommand : IRequest<bool>
    {
        public Guid ColumnId { get; set; }
        public Guid[] TaskIds { get; set; }
    }

    public class ReorderTasksCommandHandler : IRequestHandler<ReorderTasksCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ReorderTasksCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ReorderTasksCommand request, CancellationToken cancellationToken)
        {
            var column = await _context.Columns
                .Include(x => x.Board)
                .Include(x => x.Tasks.OrderBy(y => y.Order))
                .FirstOrDefaultAsync(x => x.Id == request.ColumnId, cancellationToken);

            if (column == null)
                throw new Exception("Column not found.");

            var notFoundTasks = request.TaskIds.Where(x => column.Tasks.Any(y => y.Id == x) == false).ToList();
            if (notFoundTasks.Any())
                throw new Exception("Tasks not found in given column: " + string.Join(", ", notFoundTasks));

            var notFoundTasks2 = column.Tasks.Where(x => request.TaskIds.Any(y => y == x.Id) == false).ToList();
            if (notFoundTasks2.Any())
                throw new Exception("Tasks not found in given column: " + string.Join(", ", notFoundTasks2));

            for (int i = 0; i < request.TaskIds.Length; i++)
            {
                column.Tasks.FirstOrDefault(x => x.Id == request.TaskIds[i]).Order = i;
            }

            column.Tasks.ToList().ForEach(x => x.DateOfModification = DateTime.UtcNow);
            column.DateOfModification = DateTime.UtcNow;
            column.Board.DateOfModification = DateTime.UtcNow;

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
