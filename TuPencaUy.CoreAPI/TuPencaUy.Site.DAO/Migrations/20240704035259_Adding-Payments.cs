using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddingPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Event_id = table.Column<int>(type: "int", nullable: false),
                    User_email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TransactionID = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => new { x.Event_id, x.User_email });
                    table.ForeignKey(
                        name: "FK_Payment_Event_Event_id",
                        column: x => x.Event_id,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_User_User_email",
                        column: x => x.User_email,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_User_email",
                table: "Payment",
                column: "User_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");
        }
    }
}
