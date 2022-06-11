using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capybara_api.Migrations
{
    public partial class AddRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_taskUnity_taskList_TaskListid",
                table: "taskUnity");

            migrationBuilder.RenameColumn(
                name: "TaskListid",
                table: "taskUnity",
                newName: "taskListId");

            migrationBuilder.RenameIndex(
                name: "IX_taskUnity_TaskListid",
                table: "taskUnity",
                newName: "IX_taskUnity_taskListId");

            migrationBuilder.AlterColumn<int>(
                name: "taskListId",
                table: "taskUnity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "taskList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "reminder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "note",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_taskList_userId",
                table: "taskList",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_reminder_userId",
                table: "reminder",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_note_userId",
                table: "note",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_note_user_userId",
                table: "note",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reminder_user_userId",
                table: "reminder",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_taskList_user_userId",
                table: "taskList",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_taskUnity_taskList_taskListId",
                table: "taskUnity",
                column: "taskListId",
                principalTable: "taskList",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_note_user_userId",
                table: "note");

            migrationBuilder.DropForeignKey(
                name: "FK_reminder_user_userId",
                table: "reminder");

            migrationBuilder.DropForeignKey(
                name: "FK_taskList_user_userId",
                table: "taskList");

            migrationBuilder.DropForeignKey(
                name: "FK_taskUnity_taskList_taskListId",
                table: "taskUnity");

            migrationBuilder.DropIndex(
                name: "IX_taskList_userId",
                table: "taskList");

            migrationBuilder.DropIndex(
                name: "IX_reminder_userId",
                table: "reminder");

            migrationBuilder.DropIndex(
                name: "IX_note_userId",
                table: "note");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "taskList");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "reminder");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "note");

            migrationBuilder.RenameColumn(
                name: "taskListId",
                table: "taskUnity",
                newName: "TaskListid");

            migrationBuilder.RenameIndex(
                name: "IX_taskUnity_taskListId",
                table: "taskUnity",
                newName: "IX_taskUnity_TaskListid");

            migrationBuilder.AlterColumn<int>(
                name: "TaskListid",
                table: "taskUnity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_taskUnity_taskList_TaskListid",
                table: "taskUnity",
                column: "TaskListid",
                principalTable: "taskList",
                principalColumn: "id");
        }
    }
}
