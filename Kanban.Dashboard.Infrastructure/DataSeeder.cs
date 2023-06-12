using Kanban.Dashboard.Core;
using Kanban.Dashboard.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();

        context.Boards.RemoveRange(context.Boards);
        context.Columns.RemoveRange(context.Columns);
        context.KanbanTasks.RemoveRange(context.KanbanTasks);
        context.KanbanTaskSubtask.RemoveRange(context.KanbanTaskSubtask);
        await context.SaveChangesAsync(default);
        if (!context.Boards.Any())
        {
            logger.LogInformation("Seeding boards...");

            var boards = new Board[]
            {
                new Board
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "My First Kanban Board",
                    Order = 1
                },
                new Board
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "My Second Kanban Board",
                    Order = 2
                }
            };

            context.Boards.AddRange(boards);
            await context.SaveChangesAsync(default);

            var columns = new Column[]
            {
                new Column
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "To Do", Order = 1,
                    BoardId = boards[0].Id
                },
                new Column
                {
                    Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Doing", Order = 2,
                    BoardId = boards[0].Id
                },
                new Column
                {
                    Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Done", Order = 3,
                    BoardId = boards[0].Id
                },
                new Column
                {
                    Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "To Do", Order = 1,
                    BoardId = boards[1].Id
                },
                new Column
                {
                    Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Doing", Order = 2,
                    BoardId = boards[1].Id
                },
                new Column
                {
                    Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Done", Order = 3,
                    BoardId = boards[1].Id
                }
            };

            context.Columns.AddRange(columns);
            await context.SaveChangesAsync(default);

            var kanbanTasks = new KanbanTask[]
            {
                new KanbanTask
                {
                    Id = new Guid("a0a0a0a0-a0a0-a0a0-a0a0-a0a0a0a0a0a0"),
                    Title = "Task A",
                    Description = "Task A description",
                    Order = 1,
                    ColumnId = columns[0].Id,
                    Status = "Todo",
                    UserAttached = "User1"
                },
                new KanbanTask
                {
                    Id = new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                    Title = "Task B",
                    Description = "Task B description",
                    Order = 2,
                    ColumnId = columns[0].Id,
                    Status = "Doing",
                    UserAttached = "User2"
                },
                new KanbanTask
                {
                    Id = new Guid("b0b0b0b0-b0b0-b0b0-b0b0-b0b0b0b0b0b0"),
                    Title = "Task C",
                    Description = "Task C description",
                    Order = 1,
                    ColumnId = columns[1].Id,
                    Status = "Todo",
                    UserAttached = "User1"
                },
                new KanbanTask
                {
                    Id = new Guid("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"),
                    Title = "Task D",
                    Description = "Task D description",
                    Order = 2,
                    ColumnId = columns[1].Id,
                    Status = "Done",
                    UserAttached = "User2"
                },
                new KanbanTask
                {
                    Id = new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                    Title = "Task E",
                    Description = "Task E description",
                    Order = 3,
                    ColumnId = columns[1].Id,
                    Status = "Done",
                    UserAttached = "User3"
                },
                new KanbanTask
                {
                    Id = new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"),
                    Title = "Task F",
                    Description = "Task F description",
                    Order = 4,
                    ColumnId = columns[1].Id,
                    Status = "Todo",
                    UserAttached = "User4",
                },
            };


            context.KanbanTasks.AddRange(kanbanTasks);
            await context.SaveChangesAsync(default);


            var kanbanTaskSubtasks = new KanbanTaskSubtask[]
            {
                new KanbanTaskSubtask()
                {
                    ParentId = Guid.Parse("b0b0b0b0-b0b0-b0b0-b0b0-b0b0b0b0b0b0"),
                    SubtaskId = Guid.Parse("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3")
                },
                new KanbanTaskSubtask()
                {
                    ParentId = Guid.Parse("a0a0a0a0-a0a0-a0a0-a0a0-a0a0a0a0a0a0"),
                    SubtaskId = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1")
                },
            };
            context.KanbanTaskSubtask.AddRange(kanbanTaskSubtasks);
            await context.SaveChangesAsync(default);
        }
    }
}