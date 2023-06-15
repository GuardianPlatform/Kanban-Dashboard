using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos.Requests;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class UpdateColumnCommand : IRequest
    {
        public Guid Id { get; set; }
        public CreateOrUpdateColumnRequest Column { get; set; }
    }

    public class UpdateColumnHandler : IRequestHandler<UpdateColumnCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateColumnHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
        {
            var columnDto = _mapper.Map<ColumnDto>(request.Column);
            columnDto.Id = request.Id;

            var column = await _context.Columns.FindAsync(request.Id, cancellationToken);
            if (column == null)
                throw new Exception("Column not found.");

            _mapper.Map(columnDto, column);
            column.DateOfModification = DateTime.UtcNow;

            _context.Columns.Update(column);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
