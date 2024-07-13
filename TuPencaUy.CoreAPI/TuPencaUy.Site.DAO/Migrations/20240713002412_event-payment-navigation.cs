using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class eventpaymentnavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sport_Event_EventId",
                table: "Sport");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Sport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Sport",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sport_Event_EventId",
                table: "Sport",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
