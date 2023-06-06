using System;
using System.Collections.Generic;
using System.Text;

namespace Kanban.Dashboard.Core.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Column> Columns { get; set; } = new List<Column>();
    }
}
