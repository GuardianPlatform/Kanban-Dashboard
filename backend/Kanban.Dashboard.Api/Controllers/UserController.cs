using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Features.Customer.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Dashboard.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public Task<IEnumerable<UserDto>> GetAllUsers()
        {
            return _mediator.Send(new GetAllUsersQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery() { Id = id.ToString() });
            if (result == null)
                return NoContent();
            return Ok(result);
        }
    }
}
