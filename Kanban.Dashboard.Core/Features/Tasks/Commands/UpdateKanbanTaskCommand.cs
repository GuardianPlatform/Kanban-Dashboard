using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Tasks
{
    public class UpdateKanbanTaskCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid Id { get; set; }
        public CreateOrUpdateKanbanTaskRequest KanbanTask { get; set; }
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
            var kanbanTaskDto = _mapper.Map<KanbanTaskDto>(request.KanbanTask);
            kanbanTaskDto.Id = request.Id;

            var task = await _context.KanbanTasks.FindAsync(request.Id);
            if (task == null) 
                throw new Exception("KanbanTask not found.");

            var creationDate = task.DateOfCreation;
            _mapper.Map(kanbanTaskDto, task);
            task.DateOfModification = DateTime.UtcNow;
            task.DateOfCreation = creationDate;

            var column = await _context.Columns.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == kanbanTaskDto.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception("Column not found.");

            column.DateOfModification = DateTime.UtcNow;
            column.Board.DateOfModification = DateTime.UtcNow;

            _context.KanbanTasks.Update(task);

            _context.KanbanTaskSubtask.RemoveRange(_context.KanbanTaskSubtask.Where(x => x.SubtaskId == task.Id));
            if (request.KanbanTask.ParentId != Guid.Empty)
            {
                var parentTask = await _context.KanbanTasks.FirstOrDefaultAsync(x=>x.Id == request.KanbanTask.ParentId.Value, cancellationToken: cancellationToken);
                if (parentTask == null)
                    throw new Exception("Parent task not found. ");

                _context.KanbanTaskSubtask.Add(new KanbanTaskSubtask() { ParentId = parentTask.Id, SubtaskId = task.Id });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
