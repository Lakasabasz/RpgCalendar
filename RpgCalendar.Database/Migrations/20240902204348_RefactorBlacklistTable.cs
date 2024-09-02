using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBlacklistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistsUsers_Groups_BlacklistedUserId",
                table: "BlacklistsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistsUsers_Users_EntryOwnerId",
                table: "BlacklistsUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlacklistsUsers",
                table: "BlacklistsUsers");

            migrationBuilder.RenameTable(
                name: "BlacklistsUsers",
                newName: "BlacklistUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BlacklistsUsers_BlacklistedUserId",
                table: "BlacklistUsers",
                newName: "IX_BlacklistUsers_BlacklistedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlacklistUsers",
                table: "BlacklistUsers",
                columns: new[] { "EntryOwnerId", "BlacklistedUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistUsers_Users_BlacklistedUserId",
                table: "BlacklistUsers",
                column: "BlacklistedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistUsers_Users_EntryOwnerId",
                table: "BlacklistUsers",
                column: "EntryOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistUsers_Users_BlacklistedUserId",
                table: "BlacklistUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistUsers_Users_EntryOwnerId",
                table: "BlacklistUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlacklistUsers",
                table: "BlacklistUsers");

            migrationBuilder.RenameTable(
                name: "BlacklistUsers",
                newName: "BlacklistsUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BlacklistUsers_BlacklistedUserId",
                table: "BlacklistsUsers",
                newName: "IX_BlacklistsUsers_BlacklistedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlacklistsUsers",
                table: "BlacklistsUsers",
                columns: new[] { "EntryOwnerId", "BlacklistedUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistsUsers_Groups_BlacklistedUserId",
                table: "BlacklistsUsers",
                column: "BlacklistedUserId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistsUsers_Users_EntryOwnerId",
                table: "BlacklistsUsers",
                column: "EntryOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
