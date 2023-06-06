using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Boards.Queries
{
    public class GetBoardsCommand : IRequest<IEnumerable<BoardDto>>
    {
    }

    public class GetBoardsQuery : IRequestHandler<GetBoardsCommand, IEnumerable<BoardDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBoardsQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BoardDto>> Handle(GetBoardsCommand request, CancellationToken cancellationToken)
        {
            var boards = await _context.Boards
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return _mapper.Map<IEnumerable<BoardDto>>(boards);
        }
    }
}