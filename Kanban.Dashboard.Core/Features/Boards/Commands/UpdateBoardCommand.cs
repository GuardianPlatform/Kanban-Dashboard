using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kanban.Dashboard.Core.Dtos.Requests;

namespace Kanban.Dashboard.Core.Features.Boards.Commands
{
    public class UpdateBoardCommand : IRequest
    {
        public Guid Id { get; set; }
        public CreateOrUpdateBoardRequest Board { get; set; }
    }

    public class UpdateBoardHandler : IRequestHandler<UpdateBoardCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBoardHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var boardDto = _mapper.Map<BoardDto>(request.Board);

            var board = await _context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (board == null) 
                throw new Exception("Board not found.");

            _mapper.Map(boardDto, board);
            board.DateOfModification = DateTime.UtcNow;

            _context.Boards.Update(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}