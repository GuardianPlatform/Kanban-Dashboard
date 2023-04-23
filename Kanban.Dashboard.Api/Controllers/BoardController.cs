using Kanban.Dashboard.Core.Dtos;
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
        public async Task<IActionResult> CreateBoard(BoardDto board)
        {
            var boardId = await _mediator.Send(new CreateBoardCommand()
            {
                Board = board
            });

            return CreatedAtAction(nameof(GetBoard), new { id = boardId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            var board = await _mediator.Send(new GetBoardQuery
            {
                Id = id
            });

            return board != null ? Ok(board) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(Guid id, BoardDto board)
        {
            if (id != board.Id) 
                return BadRequest();

            await _mediator.Send(new UpdateBoardCommand()
            {
                Board = board,
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(Guid id)
        {
            await _mediator.Send(new DeleteBoardCommand
            {
                Id = id
            });

            return NoContent();
        }
    }
}