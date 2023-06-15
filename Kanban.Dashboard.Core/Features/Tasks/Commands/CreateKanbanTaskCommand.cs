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
        public Guid ColumnId { get; set; }
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

            var column = await _context.Columns.AnyAsync(x => x.Id == kanbanTaskDto.ColumnId, cancellationToken);
            if (column == false)
                throw new Exception("Column not found.");

            var task = _mapper.Map<KanbanTask>(kanbanTaskDto);
            task.DateOfCreation = DateTime.UtcNow;
            task.DateOfModification = DateTime.UtcNow;
            task.UserAttached ??= "None";

            _context.KanbanTasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
