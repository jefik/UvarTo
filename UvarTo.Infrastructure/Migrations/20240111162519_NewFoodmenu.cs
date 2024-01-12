using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UvarTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewFoodmenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "FoodmenuText",
                table: "Foodmenu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

           
            migrationBuilder.DropTable(
                name: "UserRecipeSelection"
                );
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
                name: "FoodmenuText",
                table: "Foodmenu");

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
