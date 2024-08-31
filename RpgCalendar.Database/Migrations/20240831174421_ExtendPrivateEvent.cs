using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExtendPrivateEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "PrivateEvents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "PrivateEvents",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "PrivateEvents");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "PrivateEvents");
        }
    }
}
