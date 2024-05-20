using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class TeamOnSport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sport",
                table: "Team",
                type: "int",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Team");
        }
    }
}
