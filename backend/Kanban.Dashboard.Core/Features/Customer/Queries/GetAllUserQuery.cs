using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Dashboard.Core.Features.Customer.Queries
{
    public class GetAllUserQuery : IRequest<IEnumerable<User>>
    {

        public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllUserQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                var customerList = await _context.Users
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken);

                return customerList?.AsReadOnly();
            }
        }
    }
}
