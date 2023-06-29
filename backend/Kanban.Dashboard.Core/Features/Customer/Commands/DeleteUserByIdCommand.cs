using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Customer.Commands
{
    public class DeleteUserByIdCommand : IRequest<string>
    {
        public string Id { get; set; }
        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, string>
        {
            private readonly IApplicationDbContext _context;
            public DeleteUserByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
            {
                var customer = await _context.Users
                    .Where(a => a.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (customer == null)
                    return default;
                
                _context.Users.Remove(customer);
                await _context.SaveChangesAsync(cancellationToken);
                
                return customer.Id;
            }
        }
    }
}
