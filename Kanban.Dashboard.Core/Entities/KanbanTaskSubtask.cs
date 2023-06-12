using System;
namespace Kanban.Dashboard.Core.Entities
{
    public class KanbanTaskSubtask
    {
        public long Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid SubtaskId { get; set; }
        public KanbanTask Parent { get; set; } = null!;
        public KanbanTask Subtask { get; set; } = null!;
    }
}