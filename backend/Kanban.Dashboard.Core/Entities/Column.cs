using System;
using System.Collections.Generic;
using System.Text;

namespace Kanban.Dashboard.Core.Entities
{
    public class Column : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}
