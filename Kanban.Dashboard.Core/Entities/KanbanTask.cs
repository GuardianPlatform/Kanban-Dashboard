using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Entities
{
    public class KanbanTask
    {
        public Guid Id { get; set; }
        public string UserAttached { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Subtasks { get; set; } = new List<string>();
        public string Status { get; set; }
        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }

}
