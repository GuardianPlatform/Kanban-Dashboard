using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Dtos
{
    public class KanbanTaskDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Subtasks { get; set; }
        public List<string> Statuses { get; set; }
    }
}
