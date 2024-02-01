using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Customer.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public class GetAllUserQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllUserQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var customerList = _context.Users
                    .AsNoTracking()
                    .Select(x => new UserDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        Id = x.Id
                    });

                return Task.FromResult<IEnumerable<UserDto>>(customerList);
            }
        }
    }
}
