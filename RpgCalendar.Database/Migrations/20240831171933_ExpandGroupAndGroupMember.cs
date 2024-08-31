using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExpandGroupAndGroupMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "GroupsMembers",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "HeroPicture",
                table: "GroupsMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "GroupsMembers",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "GroupsMembers",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "GroupsMembers");

            migrationBuilder.DropColumn(
                name: "HeroPicture",
                table: "GroupsMembers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "GroupsMembers");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "GroupsMembers");
        }
    }
}
