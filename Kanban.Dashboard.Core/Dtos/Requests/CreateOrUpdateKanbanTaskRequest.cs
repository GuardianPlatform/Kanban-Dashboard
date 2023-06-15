﻿using System.ComponentModel.DataAnnotations;

namespace Kanban.Dashboard.Core.Dtos.Requests;

public class CreateOrUpdateKanbanTaskRequest : BaseRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string ColumnId { get; set; }
    public string? UserAttached { get; set; } = string.Empty;

}