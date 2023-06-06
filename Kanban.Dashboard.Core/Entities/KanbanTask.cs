using System;
using System.Collections.Generic;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Entities
{
    public class KanbanTask : BaseEntity
    {
        public string UserAttached { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();
        public string Status { get; set; }
        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }

}
