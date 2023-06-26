using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Features.Boards.Commands;
using Kanban.Dashboard.Core.Features.Boards.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Dashboard.Api.Controllers
{
    [ApiController]
    [Route("api/board")]
    public class BoardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BoardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBoards()
        {
            var boards = await _mediator.Send(new GetBoardsCommand());
            return Ok(boards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard(CreateBoardRequest request)
        {
            var boardId = await _mediator.Send(new CreateBoardCommand()
            {
                Board = request
            });

            return CreatedAtAction(nameof(GetBoard), new { boardId = boardId }, null);
        }

        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetBoard(Guid boardId)
        {
            var board = await _mediator.Send(new GetBoardQuery
            {
                Id = boardId
            });

            return board != null ? Ok(board) : NotFound();
        }

        [HttpPut("{boardId}")]
        public async Task<IActionResult> UpdateBoard(Guid boardId, UpdateBoardRequest request)
        {
            await _mediator.Send(new UpdateBoardCommand()
            {
                Board = request,
                Id = boardId
            });

            return NoContent();
        }

        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoard(Guid boardId)
        {
            await _mediator.Send(new DeleteBoardCommand
            {
                Id = boardId
            });

            return NoContent();
        }
    }
}