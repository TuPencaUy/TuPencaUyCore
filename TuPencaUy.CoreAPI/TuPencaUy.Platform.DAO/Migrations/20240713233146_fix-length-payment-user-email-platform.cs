using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    /// <inheritdoc />
    public partial class fixlengthpaymentuseremailplatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User_email",
                table: "Payment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User_email",
                table: "Payment",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);
        }
    }
}
