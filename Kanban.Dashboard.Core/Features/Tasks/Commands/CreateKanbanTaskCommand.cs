using AutoMapper;
using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Features.Tasks.Commands
{
    public class CreateKanbanTaskCommand : IRequest<Guid>
    {
        public CreateOrUpdateKanbanTaskRequest KanbanTask { get; set; }
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
            var kanbanTaskDto = _mapper.Map<KanbanTaskDto>(request.KanbanTask);

            var column = await _context.Columns.Include(x=>x.Board).FirstOrDefaultAsync(x=>x.Id == kanbanTaskDto.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception("Column not found.");

            var task = _mapper.Map<KanbanTask>(kanbanTaskDto);
            task.DateOfCreation = DateTime.UtcNow;
            task.DateOfModification = DateTime.UtcNow;
            task.UserAttached ??= "None";

            column.DateOfModification = DateTime.UtcNow;
            column.Board.DateOfModification = DateTime.UtcNow;

            _context.KanbanTasks.Add(task);

            if (request.KanbanTask.ParentId != Guid.Empty)
            {
                var parentTask = await _context.KanbanTasks.FirstOrDefaultAsync(x=>x.Id == request.KanbanTask.ParentId.Value, cancellationToken: cancellationToken);
                if (parentTask == null)
                    throw new Exception("Parent task not found. ");

                _context.KanbanTaskSubtask.Add(new KanbanTaskSubtask() { ParentId = parentTask.Id, SubtaskId = task.Id });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
