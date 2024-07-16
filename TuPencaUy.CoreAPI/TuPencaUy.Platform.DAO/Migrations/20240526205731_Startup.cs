using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
  /// <inheritdoc />
  public partial class Startup : Migration
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
            StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
            EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
            Comission = table.Column<double>(type: "float", nullable: true),
            Instantiable = table.Column<bool>(type: "bit", nullable: false),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            TeamType = table.Column<int>(type: "int", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Event", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Permission",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Permission", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Role",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Role", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Sport",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Tie = table.Column<bool>(type: "bit", nullable: false),
            ExactPoints = table.Column<int>(type: "int", nullable: true),
            PartialPoints = table.Column<int>(type: "int", nullable: true),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Sport", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "PermissionRole",
          columns: table => new
          {
            PermissionsId = table.Column<int>(type: "int", nullable: false),
            RolesId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RolesId });
            table.ForeignKey(
                      name: "FK_PermissionRole_Permission_PermissionsId",
                      column: x => x.PermissionsId,
                      principalTable: "Permission",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_PermissionRole_Role_RolesId",
                      column: x => x.RolesId,
                      principalTable: "Role",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "User",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            RoleId = table.Column<int>(type: "int", nullable: true),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_User", x => x.Email);
            table.ForeignKey(
                      name: "FK_User_Role_RoleId",
                      column: x => x.RoleId,
                      principalTable: "Role",
                      principalColumn: "Id");
          });

      migrationBuilder.CreateTable(
          name: "EventSport",
          columns: table => new
          {
            EventsId = table.Column<int>(type: "int", nullable: false),
            SportsId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_EventSport", x => new { x.EventsId, x.SportsId });
            table.ForeignKey(
                      name: "FK_EventSport_Event_EventsId",
                      column: x => x.EventsId,
                      principalTable: "Event",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_EventSport_Sport_SportsId",
                      column: x => x.SportsId,
                      principalTable: "Sport",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Team",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
            Logo = table.Column<byte[]>(type: "image", nullable: true),
            Sport_id = table.Column<int>(type: "int", nullable: true),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            TeamType = table.Column<int>(type: "int", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Team", x => x.Id);
            table.ForeignKey(
                      name: "FK_Team_Sport_Sport_id",
                      column: x => x.Sport_id,
                      principalTable: "Sport",
                      principalColumn: "Id");
          });

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

      migrationBuilder.CreateTable(
          name: "Match",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FirstTeam = table.Column<int>(type: "int", nullable: true),
            SecondTeam = table.Column<int>(type: "int", nullable: true),
            FirstTeamScore = table.Column<int>(type: "int", nullable: true),
            SecondTeamScore = table.Column<int>(type: "int", nullable: true),
            Date = table.Column<DateTime>(type: "datetime2", nullable: true),
            Sport_id = table.Column<int>(type: "int", nullable: true),
            EventId = table.Column<int>(type: "int", nullable: true),
            Inactive = table.Column<bool>(type: "bit", nullable: false),
            CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Match", x => x.Id);
            table.ForeignKey(
                      name: "FK_Match_Event_EventId",
                      column: x => x.EventId,
                      principalTable: "Event",
                      principalColumn: "Id");
            table.ForeignKey(
                      name: "FK_Match_Sport_Sport_id",
                      column: x => x.Sport_id,
                      principalTable: "Sport",
                      principalColumn: "Id");
            table.ForeignKey(
                      name: "FK_Match_Team_FirstTeam",
                      column: x => x.FirstTeam,
                      principalTable: "Team",
                      principalColumn: "Id");
            table.ForeignKey(
                      name: "FK_Match_Team_SecondTeam",
                      column: x => x.SecondTeam,
                      principalTable: "Team",
                      principalColumn: "Id");
          });

      migrationBuilder.CreateIndex(
          name: "IX_EventSport_SportsId",
          table: "EventSport",
          column: "SportsId");

      migrationBuilder.CreateIndex(
          name: "IX_Match_EventId",
          table: "Match",
          column: "EventId");

      migrationBuilder.CreateIndex(
          name: "IX_Match_FirstTeam",
          table: "Match",
          column: "FirstTeam");

      migrationBuilder.CreateIndex(
          name: "IX_Match_SecondTeam",
          table: "Match",
          column: "SecondTeam");

      migrationBuilder.CreateIndex(
          name: "IX_Match_Sport_id",
          table: "Match",
          column: "Sport_id");

      migrationBuilder.CreateIndex(
          name: "IX_PermissionRole_RolesId",
          table: "PermissionRole",
          column: "RolesId");

      migrationBuilder.CreateIndex(
          name: "IX_Site_UserEmail",
          table: "Site",
          column: "UserEmail");

      migrationBuilder.CreateIndex(
          name: "IX_Team_Sport_id",
          table: "Team",
          column: "Sport_id");

      migrationBuilder.CreateIndex(
          name: "IX_User_RoleId",
          table: "User",
          column: "RoleId");

      // Add sports
      migrationBuilder.Sql(
        "set identity_insert Sport on; " +
        "insert into Sport ([Id],[Name],[Tie],[ExactPoints],[PartialPoints],[Inactive],[CreationDate],[LastModificationDate]) " +
        "values " +
        "(1, 'Soccer', 1, 3, 1, 0, GETDATE(), GETDATE()), " +
        "(2, 'Basketball', 0, 15, 1, 0, GETDATE(), GETDATE()); " +
        "set identity_insert Sport off;");

      // Add roles
      migrationBuilder.Sql(
        "set identity_insert Role on; " +
        "insert into Role ([Id],[Name], [Inactive],[CreationDate],[LastModificationDate]) " +
        "values (1, 'Admin', 0, GETDATE(), GETDATE()), (2, 'BasicUser', 0, GETDATE(), GETDATE());" +
        "set identity_insert Role off;");

      // Add test users
      migrationBuilder.Sql(
        "insert into [User] (Name, Email, Password, RoleId, Inactive, CreationDate, LastModificationDate) " +
        "values ('Test basic user', 'test@basic.com', 'UwD/uVElf9TKLz/VW9S32Q==$dVucn2tjpMgcvAhSif8MisQZORWzv6KAkqhUPXAHT3M=', 2, 0, GETDATE(), GETDATE()), " +
        "('Admin user', 'test@admin.com', 'UwD/uVElf9TKLz/VW9S32Q==$dVucn2tjpMgcvAhSif8MisQZORWzv6KAkqhUPXAHT3M=', 1, 0, GETDATE(), GETDATE())");

      
    }


    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      // Delete users
      migrationBuilder.Sql("delete from [User];");
      // Delete roles
      migrationBuilder.Sql("delete from Role;");
      // Delete sports
      migrationBuilder.Sql("delete from Sport;");


      migrationBuilder.DropTable(
          name: "EventSport");

      migrationBuilder.DropTable(
          name: "Match");

      migrationBuilder.DropTable(
          name: "PermissionRole");

      migrationBuilder.DropTable(
          name: "Site");

      migrationBuilder.DropTable(
          name: "Event");

      migrationBuilder.DropTable(
          name: "Team");

      migrationBuilder.DropTable(
          name: "Permission");

      migrationBuilder.DropTable(
          name: "User");

      migrationBuilder.DropTable(
          name: "Sport");

      migrationBuilder.DropTable(
          name: "Role");
    }
  }
}
