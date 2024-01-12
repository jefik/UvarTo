using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UvarTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRecipeText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "RecipeText",
               table: "Recept",
               type: "nvarchar(max)",
               nullable: false,
               defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeText",
                table: "Recept");
        }
    }
}
