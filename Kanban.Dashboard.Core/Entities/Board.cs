using System;
using System.Collections.Generic;
using System.Text;

namespace Kanban.Dashboard.Core.Entities
{
    public class Board
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public ICollection<Column> Columns { get; set; }
    }
}
