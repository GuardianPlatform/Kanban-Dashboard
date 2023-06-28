using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Entities
{
    public class User : ApplicationUser
    {
        public string Email { get; set; }
        public string Login { get; set; }

        public List<Board> Boards { get; set; }
        [IgnoreDataMember]
        public List<BoardUsers> BoardsUsers { get; set; }

    }
}
