using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class PrivateEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivateEvents",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SimpleAbsence = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateEvents", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_PrivateEvents_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateEvents_OwnerId",
                table: "PrivateEvents",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateEvents");
        }
    }
}
