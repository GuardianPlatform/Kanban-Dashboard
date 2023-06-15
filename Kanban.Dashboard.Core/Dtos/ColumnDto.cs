using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kanban.Dashboard.Core.Dtos
{
    public class ColumnDto : BaseDto
    {
        public string Name { get; set; }
        public ICollection<KanbanTaskDto> Tasks { get; set; } = new List<KanbanTaskDto>();

        [IgnoreDataMember]
        public Guid BoardId { get; set; }
    }
}
