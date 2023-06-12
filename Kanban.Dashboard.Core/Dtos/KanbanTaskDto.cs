using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Dtos
{
    public class KanbanTaskDto : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<SubtaskDto> Subtasks { get; set; } = new List<SubtaskDto>();
        public List<SubtaskDto> Parents { get; set; } = new List<SubtaskDto>();
        public string Status { get; set; }
    }
}
