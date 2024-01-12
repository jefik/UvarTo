using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UvarTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedRecipes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Recept",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Recept");
        }
    }
}
