using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Dashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubtasksMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "KanbanTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfModification",
                table: "KanbanTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Columns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfModification",
                table: "Columns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Boards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfModification",
                table: "Boards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "KanbanTaskSubtask",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubtaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanTaskSubtask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KanbanTaskSubtask_KanbanTasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "KanbanTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KanbanTaskSubtask_KanbanTasks_SubtaskId",
                        column: x => x.SubtaskId,
                        principalTable: "KanbanTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KanbanTaskSubtask_ParentId",
                table: "KanbanTaskSubtask",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanTaskSubtask_SubtaskId",
                table: "KanbanTaskSubtask",
                column: "SubtaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KanbanTaskSubtask");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "KanbanTasks");

            migrationBuilder.DropColumn(
                name: "DateOfModification",
                table: "KanbanTasks");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "DateOfModification",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "DateOfModification",
                table: "Boards");
        }
    }
}
