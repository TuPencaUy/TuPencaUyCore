using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class addeventIdfktomatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Event_EventId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_EventId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Match");

            migrationBuilder.AddColumn<int>(
                name: "RefTeam",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefSport",
                table: "Sport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Event_id",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<int>(
                name: "RefMatch",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefEvent",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Match_Event_id",
                table: "Match",
                column: "Event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Event_Event_id",
                table: "Match",
                column: "Event_id",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Event_Event_id",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_Event_id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "RefTeam",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "RefSport",
                table: "Sport");

            migrationBuilder.DropColumn(
                name: "Event_id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "RefMatch",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "RefEvent",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_EventId",
                table: "Match",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Event_EventId",
                table: "Match",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
