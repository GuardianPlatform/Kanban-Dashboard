using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Dtos.Requests;

public class CreateOrUpdateKanbanTaskRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Guid ColumnId { get; set; }
    public string? UserAttached { get; set; } = string.Empty;
    public Guid? ParentId { get; set; } = Guid.Empty;
}