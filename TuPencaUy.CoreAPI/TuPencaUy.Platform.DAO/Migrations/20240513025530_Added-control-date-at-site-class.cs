using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class Addedcontroldateatsiteclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Domain = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ConnectionString = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    AccessType = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: true),
                    UserEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Site_User_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "User",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Site_UserEmail",
                table: "Site",
                column: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Site");
        }
    }
}
