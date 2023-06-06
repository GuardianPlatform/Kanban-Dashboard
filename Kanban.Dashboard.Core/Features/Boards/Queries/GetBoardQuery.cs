using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Boards.Queries
{
    public class GetBoardQuery : IRequest<BoardDto>
    {
        public Guid Id { get; set; }
    }

    public class GetBoardHandler : IRequestHandler<GetBoardQuery, BoardDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBoardHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards
                .AsNoTracking()
                .Include(b => b.Columns)
                .ThenInclude(c => c.Tasks).ThenInclude(x => x.Subtasks)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);

            return board != null ? _mapper.Map<BoardDto>(board) : throw new Exception("Board not found");
        }
    }
}
