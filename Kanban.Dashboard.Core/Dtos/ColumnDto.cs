using System;
using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Dtos
{
    public class ColumnDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public ICollection<KanbanTaskDto> Tasks { get; set; }
    }
}
