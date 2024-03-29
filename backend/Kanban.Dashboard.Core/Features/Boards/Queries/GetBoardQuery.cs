﻿using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            var query = _context.Boards
                .AsNoTracking()
                .OrderBy(x=>x.Order)
                .Include(b => b.Columns.OrderBy(x => x.Order)).ThenInclude(c => c.Tasks.OrderBy(x => x.Order)).ThenInclude(x => x.Parents.OrderBy(x => x.Order))
                .OrderBy(x => x.Order)
                .Include(x => x.Columns.OrderBy(x => x.Order)).ThenInclude(c => c.Tasks.OrderBy(x => x.Order)).ThenInclude(x => x.Subtasks.OrderBy(x => x.Order))
                .OrderBy(x => x.Order)
                .Include(b => b.Columns.OrderBy(x=>x.Order)).ThenInclude(c => c.Tasks.OrderBy(x => x.Order))
                .OrderBy(x => x.Order);

            var board = await query.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);

            if(board == null)
                throw new Exception("Board not found.");

            var result = _mapper.Map<BoardDto>(board);
            result.TotalNumberOfTasks = await _context.KanbanTasks
                .Include(x=>x.Column)
                .CountAsync(x=>x.Column.BoardId == board.Id, cancellationToken)
                .ConfigureAwait(false);

            return board != null ? result : null!;
        }
    }
}