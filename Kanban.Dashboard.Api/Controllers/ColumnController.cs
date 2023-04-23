using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Features.Columns.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[ApiController]
[Route("api/board/{boardId}/column")]
public class ColumnController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateColumn(Guid boardId, ColumnDto column)
    {
        var columnId = await _mediator.Send(new CreateColumnCommand()
        {
            BoardId = boardId,
            Column = column
        });

        return CreatedAtAction(nameof(CreateColumn), new { id = columnId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColumn(Guid boardId, Guid id, ColumnDto column)
    {
        if (column.Id != id) 
            return BadRequest();

        await _mediator.Send(new UpdateColumnCommand()
        {
            Column = column,
            BoardId = boardId
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColumn(Guid boardId, Guid id)
    {
        await _mediator.Send(new DeleteColumnCommand
        {
            BoardId = boardId,
            Id = id
        });

        return NoContent();
    }
}