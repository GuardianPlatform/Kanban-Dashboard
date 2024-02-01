using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Customer.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public string Id { get; set; }

        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
        {
            private readonly IApplicationDbContext _context;
            public GetUserByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var customer = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == request.Id);

                if (customer == null)
                    return Task.FromResult<UserDto?>(null);

                return Task.FromResult(new UserDto()
                {
                    FirstName = customer?.FirstName,
                    LastName = customer?.LastName,
                    UserName = customer?.UserName,
                    Id = customer?.Id
                });
            }
        }
    }
}
