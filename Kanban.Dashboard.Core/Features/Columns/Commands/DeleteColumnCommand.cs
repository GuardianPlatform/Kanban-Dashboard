using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class DeleteColumnCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public Guid Id { get; set; }
    }

    public class DeleteColumnHandler : IRequestHandler<DeleteColumnCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteColumnHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards.AnyAsync(x => x.Id == request.BoardId, cancellationToken);
            if (board == false)
                throw new Exception("Board not found.");

            var column = await _context.Columns.FindAsync(request.Id);
            if (column == null) 
                throw new Exception("Column not found.");

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
