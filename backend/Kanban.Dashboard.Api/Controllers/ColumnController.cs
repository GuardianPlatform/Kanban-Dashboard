using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Features.Columns.Commands;
using Kanban.Dashboard.Core.Features.Tasks.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Api.Controllers;

[ApiController]
[Route("api/column/")]
[Authorize]
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

        return CreatedAtAction(nameof(CreateColumn), new { columnId = columnId }, null);
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

    [HttpPost("{columnId}/reorder")]
    public async Task<IActionResult> ReorderTasks(Guid columnId, Guid[] taskIds)
    {
        var result = await _mediator.Send(new ReorderTasksCommand()
        {
            ColumnId = columnId,
            TaskIds = taskIds
        });

        return Ok(result);
    }
}