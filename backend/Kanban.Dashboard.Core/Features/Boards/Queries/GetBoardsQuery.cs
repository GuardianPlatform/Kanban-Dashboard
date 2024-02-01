using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kanban.Dashboard.Core.Features.Boards.Queries
{
    public class GetBoardsCommand : IRequest<IEnumerable<BoardDto>>
    {
    }

    public class GetBoardsQuery : IRequestHandler<GetBoardsCommand, IEnumerable<BoardDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetBoardsQuery(IApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<BoardDto>> Handle(GetBoardsCommand request, CancellationToken cancellationToken)
        {
            var boards = await _context.Boards
                .AsNoTracking()
                .Where(x => x.Users.Any(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name))
                .OrderBy(x => x.Order)
                .ThenByDescending(x => x.DateOfModification)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var result = _mapper.Map<List<BoardDto>>(boards);

            foreach (var board in result)
            {
                board.TotalNumberOfTasks = await _context.KanbanTasks
                    .Include(x => x.Column)
                    .CountAsync(x => x.Column.BoardId == board.Id, cancellationToken)
                    .ConfigureAwait(false);
            }

            return result;
        }
    }
}