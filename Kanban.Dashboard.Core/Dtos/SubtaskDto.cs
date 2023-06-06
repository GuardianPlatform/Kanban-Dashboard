using System;
using System.Text.Json.Serialization;

namespace Kanban.Dashboard.Core.Dtos
{
    public class SubtaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted => Status == "Done";

        [JsonIgnore]
        public string Status { get; set; }
    }
}
