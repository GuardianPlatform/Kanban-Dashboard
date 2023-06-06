using System;
using System.Text.Json.Serialization;

namespace Kanban.Dashboard.Core.Entities
{
    public class Subtask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        [JsonIgnore]
        public KanbanTask KanbanTask { get; set; }
    }
}
