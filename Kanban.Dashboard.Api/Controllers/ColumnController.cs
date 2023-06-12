using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Features.Columns.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/column/")]
public class ColumnController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateColumn(CreateOrUpdateColumnRequest request)
    {
        var columnId = await _mediator.Send(new CreateColumnCommand()
        {
            Column = request
        });

        return CreatedAtAction(nameof(CreateColumn), new { id = columnId }, null);
    }

    [HttpPut("{columnId}")]
    public async Task<IActionResult> UpdateColumn(Guid columnId, CreateOrUpdateColumnRequest request)
    {
        await _mediator.Send(new UpdateColumnCommand()
        {
            Id = columnId,
            Column = request,
        });

        return NoContent();
    }

    [HttpDelete("{columnId}")]
    public async Task<IActionResult> DeleteColumn(Guid columnId)
    {
        await _mediator.Send(new DeleteColumnCommand
        {
            Id = columnId
        });

        return NoContent();
    }
}