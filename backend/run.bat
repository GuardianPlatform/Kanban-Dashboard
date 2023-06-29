dotnet build ./Kanban.Dashboard.Api.sln
dotnet ef database update --context ApplicationDbContext --project ./Kanban.Dashboard.Api/Kanban.Dashboard.Api.csproj
start dotnet run --project ./Kanban.Dashboard.Api/Kanban.Dashboard.Api.csproj