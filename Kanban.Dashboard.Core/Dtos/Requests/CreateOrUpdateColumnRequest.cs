namespace Kanban.Dashboard.Core.Dtos.Requests
{
    public class CreateOrUpdateColumnRequest : BaseRequest
    {
        public string Name { get; set; }
        public string BoardId { get; set; }
    }
}
