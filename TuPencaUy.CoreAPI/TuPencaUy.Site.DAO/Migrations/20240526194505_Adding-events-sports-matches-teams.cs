using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class Addingeventssportsmatchesteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Comission = table.Column<double>(type: "float", nullable: true),
                    Instantiable = table.Column<bool>(type: "bit", nullable: false),
                    TeamType = table.Column<int>(type: "int", nullable: true),
                    UserEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseEvent_User_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "User",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_BaseEvent_Id",
                        column: x => x.Id,
                        principalTable: "BaseEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Tie = table.Column<bool>(type: "bit", nullable: false),
                    ExactPoints = table.Column<int>(type: "int", nullable: true),
                    PartialPoints = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sport_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(50)", maxLength: 50, nullable: true),
                    Sport = table.Column<int>(type: "int", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeamType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Sport_Sport",
                        column: x => x.Sport,
                        principalTable: "Sport",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstTeam = table.Column<int>(type: "int", nullable: true),
                    SecondTeam = table.Column<int>(type: "int", nullable: true),
                    FirstTeamScore = table.Column<int>(type: "int", nullable: true),
                    SecondTeamScore = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sport = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Sport_Sport",
                        column: x => x.Sport,
                        principalTable: "Sport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Team_FirstTeam",
                        column: x => x.FirstTeam,
                        principalTable: "Team",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Team_SecondTeam",
                        column: x => x.SecondTeam,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_EventId",
                table: "User",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEvent_UserEmail",
                table: "BaseEvent",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Match_EventId",
                table: "Match",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_FirstTeam",
                table: "Match",
                column: "FirstTeam");

            migrationBuilder.CreateIndex(
                name: "IX_Match_SecondTeam",
                table: "Match",
                column: "SecondTeam");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Sport",
                table: "Match",
                column: "Sport");

            migrationBuilder.CreateIndex(
                name: "IX_Sport_EventId",
                table: "Sport",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Sport",
                table: "Team",
                column: "Sport");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Event_EventId",
                table: "User",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Event_EventId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "BaseEvent");

            migrationBuilder.DropIndex(
                name: "IX_User_EventId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "User");
        }
    }
}
