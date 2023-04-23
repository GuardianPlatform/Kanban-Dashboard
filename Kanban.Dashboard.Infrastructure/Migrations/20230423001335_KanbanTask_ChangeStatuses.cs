using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Dashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KanbanTask_ChangeStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Statuses",
                table: "KanbanTasks",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "KanbanTasks",
                newName: "Statuses");
        }
    }
}
