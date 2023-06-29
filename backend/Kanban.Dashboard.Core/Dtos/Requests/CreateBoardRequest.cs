namespace Kanban.Dashboard.Core.Dtos.Requests;

public class CreateBoardRequest : BaseRequest
{
    public string Name { get; set; }
    public CreateOrUpdateColumnsForBoardRequest[] Columns { get; set; }
}

public class UpdateBoardRequest : BaseRequest
{
    public string Name { get; set; }
}

public class CreateOrUpdateColumnsForBoardRequest : BaseRequest
{
    public string Name { get; set; }
}