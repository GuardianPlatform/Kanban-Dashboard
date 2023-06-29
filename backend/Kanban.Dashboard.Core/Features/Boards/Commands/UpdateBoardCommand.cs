using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Entities;

namespace Kanban.Dashboard.Core.Features.Boards.Commands
{
    public class UpdateBoardCommand : IRequest
    {
        public Guid Id { get; set; }
        public UpdateBoardRequest Board { get; set; }
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
            boardDto.Id = request.Id;

            var board = await _context.Boards
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

            var creationDate = board.DateOfCreation;
            if (board == null) 
                throw new Exception("Board not found.");

            _mapper.Map(boardDto, board);
            board.DateOfModification = DateTime.UtcNow;
            board.DateOfCreation = creationDate;

            _context.Boards.Update(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}