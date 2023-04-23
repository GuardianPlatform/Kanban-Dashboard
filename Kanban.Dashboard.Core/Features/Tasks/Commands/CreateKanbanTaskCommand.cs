using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class CreateKanbanTaskCommand : IRequest<Guid>
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public KanbanTaskDto KanbanTask { get; set; }
    }

    public class CreateKanbanTaskHandler : IRequestHandler<CreateKanbanTaskCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateKanbanTaskHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateKanbanTaskCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards.AnyAsync(x => x.Id == request.BoardId, cancellationToken);
            if (board == false)
                throw new Exception("Board not found.");

            var column = await _context.Columns.AnyAsync(x => x.Id == request.ColumnId, cancellationToken);
            if (column == false)
                throw new Exception("Column not found.");

            var task = _mapper.Map<KanbanTask>(request.KanbanTask);
            task.Column.BoardId = request.BoardId;
            task.ColumnId = request.ColumnId;

            _context.KanbanTasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
