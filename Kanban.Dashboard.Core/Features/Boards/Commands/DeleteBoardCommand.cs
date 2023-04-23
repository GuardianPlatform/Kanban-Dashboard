using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Boards.Commands
{
    public class DeleteBoardCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBoardHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards
                .Include(b => b.Columns)
                .ThenInclude(c => c.Tasks)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (board == null) 
                throw new Exception("Board not found.");

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
