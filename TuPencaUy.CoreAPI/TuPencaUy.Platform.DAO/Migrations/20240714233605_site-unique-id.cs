using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
  /// <inheritdoc />
  public partial class siteuniqueid : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "UniqueID",
          table: "Site",
          type: "varchar(50)",
          maxLength: 50,
          nullable: true)
          .Annotation("Relational:ColumnOrder", 4);// Add "Copa america" teams

      string teamsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dump", "teams.sql");
      string eventsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dump", "events.sql");
      string matchesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dump", "matches.sql");
      string teamsSQL = File.ReadAllText(teamsPath);
      string eventsSQL = File.ReadAllText(eventsPath);
      string matchesSQL = File.ReadAllText(matchesPath);

      migrationBuilder.Sql(eventsSQL);
      migrationBuilder.Sql(teamsSQL);
      migrationBuilder.Sql(matchesSQL);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      // Delete teams
      migrationBuilder.Sql("delete from Team;");
      migrationBuilder.Sql("delete from Event;");
      migrationBuilder.Sql("delete from Match;");

      migrationBuilder.DropColumn(
          name: "UniqueID",
          table: "Site");
    }
  }
}
