using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixRenamedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "PrivateEvents",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "PrivateEvents",
                newName: "EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "PrivateEvents",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "PrivateEvents",
                newName: "End");
        }
    }
}
