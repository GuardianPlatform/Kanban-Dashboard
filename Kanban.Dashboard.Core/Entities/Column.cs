using System;
using System.Collections.Generic;
using System.Text;

namespace Kanban.Dashboard.Core.Entities
{
    public class Column
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public ICollection<KanbanTask> Tasks { get; set; }
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}
