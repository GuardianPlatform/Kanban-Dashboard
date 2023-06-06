using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Boards.Commands
{
    public class CreateBoardCommand : IRequest<Guid>
    {
        public BoardDto Board { get; set; }
    }

    public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateBoardHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = _mapper.Map<Board>(request.Board);
            board.DateOfCreation = DateTime.UtcNow;

            _context.Boards.Add(board);
            await _context.SaveChangesAsync(cancellationToken);
            return board.Id;
        }
    }
}
