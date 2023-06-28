using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Entities
{
    public class BoardUsers
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}
