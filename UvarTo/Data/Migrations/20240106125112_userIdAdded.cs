using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UvarTo.Data.Migrations
{
    /// <inheritdoc />
    public partial class userIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tips",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Tips",
                type: "string",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Recept",
                type: "string",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Foodmenu",
                type: "string",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Tips");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Recept");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Foodmenu");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tips",
                newName: "id");
        }
    }
}
