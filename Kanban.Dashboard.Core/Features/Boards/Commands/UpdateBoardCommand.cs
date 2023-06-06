using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Boards.Commands
{
    public class UpdateBoardCommand : IRequest
    {
        public BoardDto Board { get; set; }
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
            var board = await _context.Boards
                .FindAsync(request.Board.Id);

            if (board == null) 
                throw new Exception("Board not found.");

            _mapper.Map(request.Board, board);
            board.DateOfModification = DateTime.UtcNow;

            _context.Boards.Update(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}