using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    /// <inheritdoc />
    public partial class addbets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    Event_id = table.Column<int>(type: "int", nullable: false),
                    Match_id = table.Column<int>(type: "int", nullable: false),
                    User_email = table.Column<string>(type: "varchar(50)", nullable: false),
                    ScoreFirstTeam = table.Column<int>(type: "int", nullable: false),
                    ScoreSecondTeam = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => new { x.Match_id, x.Event_id, x.User_email });
                    table.ForeignKey(
                        name: "FK_Bet_Event_Event_id",
                        column: x => x.Event_id,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bet_Match_Match_id",
                        column: x => x.Match_id,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Bet_User_User_email",
                        column: x => x.User_email,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_Event_id",
                table: "Bet",
                column: "Event_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_User_email",
                table: "Bet",
                column: "User_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet");
        }
    }
}
