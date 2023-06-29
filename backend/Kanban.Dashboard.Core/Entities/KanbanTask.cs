using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Entities
{
    public class KanbanTask : BaseEntity
    {
        public string UserAttached { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<KanbanTask> Parents { get; set; } = new List<KanbanTask>();
        public ICollection<KanbanTask> Subtasks { get; set; } = new List<KanbanTask>();
        public string Status { get; set; }
        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }

}
