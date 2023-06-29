using System;

namespace Kanban.Dashboard.Core.Dtos.Requests
{
    public class SwitchColumnRequest
    {
        public Guid ColumnTargetId { get; set; }
    }
}
