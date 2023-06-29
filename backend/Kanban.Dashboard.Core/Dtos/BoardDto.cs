using System.Collections.Generic;

namespace Kanban.Dashboard.Core.Dtos
{
    public class BoardDto : BaseDto
    {
        public string Name { get; set; }
        public int TotalNumberOfTasks { get; set; }
        public ICollection<ColumnDto> Columns { get; set; } = new List<ColumnDto>();
    }
}
