using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PA.Reflect.Daily.Infrastructure.Migrations;

  /// <inheritdoc />
  public partial class Init : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "Users",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  Sub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  ProfilePictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Users", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "Reflections",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  UserId = table.Column<int>(type: "int", nullable: false),
                  Date = table.Column<DateTime>(type: "datetime2", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Reflections", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Reflections_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Notes",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Filepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  ReflectionId = table.Column<int>(type: "int", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Notes", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Notes_Reflections_ReflectionId",
                      column: x => x.ReflectionId,
                      principalTable: "Reflections",
                      principalColumn: "Id");
              });

          migrationBuilder.CreateIndex(
              name: "IX_Notes_ReflectionId",
              table: "Notes",
              column: "ReflectionId");

          migrationBuilder.CreateIndex(
              name: "IX_Reflections_UserId",
              table: "Reflections",
              column: "UserId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "Notes");

          migrationBuilder.DropTable(
              name: "Reflections");

          migrationBuilder.DropTable(
              name: "Users");
      }
  }
