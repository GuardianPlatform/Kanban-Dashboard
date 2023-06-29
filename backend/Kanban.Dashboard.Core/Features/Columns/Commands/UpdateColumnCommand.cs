using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos.Requests;
using Microsoft.EntityFrameworkCore;

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

            var column = await _context.Columns.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
            if (column == null)
                throw new Exception("Column not found.");

            var board = await _context.Boards.FirstOrDefaultAsync(x => x.Id == request.Column.BoardId, cancellationToken);
            if (board == null)
                throw new Exception("Board not found.");


            var creationDate = column.DateOfCreation;
            _mapper.Map(columnDto, column);
            column.DateOfModification = DateTime.UtcNow;
            column.DateOfCreation = creationDate;
            board.DateOfModification = DateTime.UtcNow;

            _context.Columns.Update(column);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
