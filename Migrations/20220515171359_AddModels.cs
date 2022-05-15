using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capybara_api.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Theme = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotificationEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskList", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserPreferenceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Preference_UserPreferenceId",
                        column: x => x.UserPreferenceId,
                        principalTable: "Preference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskUnity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsComplete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TaskListId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskUnity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskUnity_TaskList_TaskListId",
                        column: x => x.TaskListId,
                        principalTable: "TaskList",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUnity_TaskListId",
                table: "TaskUnity",
                column: "TaskListId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserPreferenceId",
                table: "User",
                column: "UserPreferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "TaskUnity");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "TaskList");

            migrationBuilder.DropTable(
                name: "Preference");
        }
    }
}
