using System;

namespace Kanban.Dashboard.Core.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfModification { get; set; }
    }
}
