using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Tasks
{
    public class UpdateKanbanTaskCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid Id { get; set; }
        public KanbanTaskDto KanbanTask { get; set; }
    }

    public class UpdateKanbanTaskHandler : IRequestHandler<UpdateKanbanTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateKanbanTaskHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateKanbanTaskCommand request, CancellationToken cancellationToken)
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

            _mapper.Map(request.KanbanTask, task);
            _context.KanbanTasks.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
