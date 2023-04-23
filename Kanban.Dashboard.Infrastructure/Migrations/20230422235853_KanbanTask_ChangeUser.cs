using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Dashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KanbanTask_ChangeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "KanbanTasks");

            migrationBuilder.AddColumn<string>(
                name: "UserAttached",
                table: "KanbanTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAttached",
                table: "KanbanTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "KanbanTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
