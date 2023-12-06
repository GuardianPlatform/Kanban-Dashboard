using Kanban.Dashboard.Core.Dtos.Requests;
using Kanban.Dashboard.Core.Features.Tasks.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Dashboard.Api.Controllers;

[ApiController]
[Route("api/task/")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateOrUpdateKanbanTaskRequest request)
    {
        var taskId = await _mediator.Send(new CreateKanbanTaskCommand()
        {
            KanbanTask = request
        });

        return CreatedAtAction(nameof(CreateTask), new { taskId = taskId }, null);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, CreateOrUpdateKanbanTaskRequest request)
    {
        await _mediator.Send(new UpdateKanbanTaskCommand()
        {
            Id = taskId,
            KanbanTask = request
        });

        return NoContent();
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        await _mediator.Send(new DeleteKanbanTaskCommand
        {
            Id = taskId,
        });

        return NoContent();
    }

    [Obsolete]
    [HttpPost("{taskId}/switch-column")]
    public async Task<IActionResult> SwitchColumn(Guid taskId, SwitchColumnRequest request)
    {
        var result = await _mediator.Send(new SwitchTaskColumnCommand()
        {
            TaskId = taskId,
            ColumnTargetId = request.ColumnTargetId
        });

        return Ok(result);
    }

    [HttpPost("{taskId}/move")]
    public async Task<IActionResult> Move(Guid taskId,  MoveTaskRequest request)
    {
        var result = await _mediator.Send(new MoveTaskCommand()
        {
            TaskId = taskId,
            ColumnTargetId = request.ColumnTargetId,
            Order = request.Order
        });

        return Ok(result);
    }
}