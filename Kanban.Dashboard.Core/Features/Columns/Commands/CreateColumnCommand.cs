using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos.Requests;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class CreateColumnCommand : IRequest<Guid>
    {
        public CreateOrUpdateColumnRequest Column { get; set; }
    }

    public class CreateColumnHandler : IRequestHandler<CreateColumnCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateColumnHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
        {
            var columnDto = _mapper.Map<ColumnDto>(request.Column);

            var column = _mapper.Map<Column>(columnDto);
            column.DateOfCreation = DateTime.UtcNow;
            column.DateOfModification = DateTime.UtcNow;

            _context.Columns.Add(column);
            await _context.SaveChangesAsync(cancellationToken);

            return column.Id;
        }
    }
}
