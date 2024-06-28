using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class addeventstosports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sport_Event_EventId",
                table: "Sport");

            migrationBuilder.DropIndex(
                name: "IX_Sport_EventId",
                table: "Sport");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Sport");

            migrationBuilder.CreateTable(
                name: "EventSport",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    SportsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSport", x => new { x.EventsId, x.SportsId });
                    table.ForeignKey(
                        name: "FK_EventSport_Event_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSport_Sport_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSport_SportsId",
                table: "EventSport",
                column: "SportsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSport");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Sport",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sport_EventId",
                table: "Sport",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sport_Event_EventId",
                table: "Sport",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
