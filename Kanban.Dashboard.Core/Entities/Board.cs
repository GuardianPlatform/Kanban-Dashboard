using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kanban.Dashboard.Core.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Column> Columns { get; set; } = new List<Column>();

        public List<User> Users { get; set; }
        [IgnoreDataMember]
        public List<BoardUsers> BoardUsers { get; set; }
    }
}
