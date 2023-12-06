using System;

namespace Kanban.Dashboard.Core.Dtos.Requests
{
    public class MoveTaskRequest
    {
        public Guid ColumnTargetId { get; set; }
        public int Order { get; set; }
    }
}
