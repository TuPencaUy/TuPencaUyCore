using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingEventsMatchsListsEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamType",
                table: "Team",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamType",
                table: "Event",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamType",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "TeamType",
                table: "Event");
        }
    }
}
