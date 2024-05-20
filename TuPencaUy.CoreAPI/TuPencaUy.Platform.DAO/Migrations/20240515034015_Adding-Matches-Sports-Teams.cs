using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddingMatchesSportsTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FirstTeam = table.Column<int>(type: "int", nullable: true),
                    SecondTeam = table.Column<int>(type: "int", nullable: true),
                    FirstTeamScore = table.Column<int>(type: "int", nullable: true),
                    SecondTeamScore = table.Column<int>(type: "int", nullable: true),
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
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_EventId",
                table: "Match",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sport_EventId",
                table: "Sport",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
