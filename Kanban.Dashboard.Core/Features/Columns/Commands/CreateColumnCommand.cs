using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class CreateColumnCommand : IRequest<Guid>
    {
        public Guid BoardId { get; set; }
        public ColumnDto Column { get; set; }
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
            var board = await _context.Boards.AnyAsync(x => x.Id == request.BoardId, cancellationToken);
            if(board == false)
                throw new Exception("Board not found.");

            var column = _mapper.Map<Column>(request.Column);
            column.DateOfCreation = DateTime.UtcNow;
            column.BoardId = request.BoardId;

            _context.Columns.Add(column);
            await _context.SaveChangesAsync(cancellationToken);

            return column.Id;
        }
    }
}
