namespace Kanban.Dashboard.Core.Dtos.Requests;

public class CreateOrUpdateBoardRequest : BaseRequest
{
    public string Name { get; set; }
    public CreateOrUpdateColumnRequest[] Columns { get; set; }
}