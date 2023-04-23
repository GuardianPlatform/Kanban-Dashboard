using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Features.Tasks;
using Kanban.Dashboard.Core.Features.Tasks.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/board/{boardId}/column/{columnId}/task")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(Guid boardId, Guid columnId, KanbanTaskDto kanbanTask)
    {
        var taskId = await _mediator.Send(new CreateKanbanTaskCommand()
        {
            BoardId = boardId,
            ColumnId = columnId,
            KanbanTask = kanbanTask
        });

        return CreatedAtAction(nameof(CreateTask), new { id = taskId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid boardId, Guid columnId, Guid id, KanbanTaskDto kanbanTask)
    {
        if (kanbanTask.Id != id)
            return BadRequest();

        await _mediator.Send(new UpdateKanbanTaskCommand()
        {
            BoardId = boardId,
            ColumnId = columnId,
            KanbanTask = kanbanTask
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid boardId, Guid columnId, Guid id)
    {
        await _mediator.Send(new DeleteKanbanTaskCommand
        {
            Id = id,
            BoardId = boardId,
            ColumnId = columnId
        });

        return NoContent();
    }
}