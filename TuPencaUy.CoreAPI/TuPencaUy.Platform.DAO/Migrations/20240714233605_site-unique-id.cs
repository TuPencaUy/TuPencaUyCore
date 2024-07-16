using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;

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

      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

      string teamsSQL = File.ReadAllText(configuration["Dump:Teams"]);
      string eventsSQL = File.ReadAllText(configuration["Dump:Events"]);
      string matchesSQL = File.ReadAllText(configuration["Dump:Matches"]);

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
