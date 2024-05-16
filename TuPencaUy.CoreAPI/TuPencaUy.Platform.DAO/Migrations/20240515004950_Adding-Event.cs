using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddingEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Comission = table.Column<double>(type: "float", nullable: false),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
