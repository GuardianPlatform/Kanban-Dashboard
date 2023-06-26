using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class DeleteColumnCommand : IRequest
    {
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
            var column = await _context.Columns.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);
            if (column == null) 
                throw new Exception("Column not found.");

            column.Board.DateOfModification = DateTime.UtcNow;

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
