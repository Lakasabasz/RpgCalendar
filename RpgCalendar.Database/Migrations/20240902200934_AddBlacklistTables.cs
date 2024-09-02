using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBlacklistTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistGroups",
                columns: table => new
                {
                    EntryOwnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BlacklistedGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistGroups", x => new { x.EntryOwnerId, x.BlacklistedGroupId });
                    table.ForeignKey(
                        name: "FK_BlacklistGroups_Groups_BlacklistedGroupId",
                        column: x => x.BlacklistedGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistGroups_Users_EntryOwnerId",
                        column: x => x.EntryOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlacklistsUsers",
                columns: table => new
                {
                    EntryOwnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BlacklistedUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistsUsers", x => new { x.EntryOwnerId, x.BlacklistedUserId });
                    table.ForeignKey(
                        name: "FK_BlacklistsUsers_Groups_BlacklistedUserId",
                        column: x => x.BlacklistedUserId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistsUsers_Users_EntryOwnerId",
                        column: x => x.EntryOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistGroups_BlacklistedGroupId",
                table: "BlacklistGroups",
                column: "BlacklistedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistsUsers_BlacklistedUserId",
                table: "BlacklistsUsers",
                column: "BlacklistedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistGroups");

            migrationBuilder.DropTable(
                name: "BlacklistsUsers");
        }
    }
}
