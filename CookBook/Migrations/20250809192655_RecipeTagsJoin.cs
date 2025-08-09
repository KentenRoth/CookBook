using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookBook.Migrations
{
    /// <inheritdoc />
    public partial class RecipeTagsJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Recipes_RecipesId",
                table: "RecipeTag");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Tags_TagsId",
                table: "RecipeTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "RecipeTag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "RecipesId",
                table: "RecipeTag",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTag_TagsId",
                table: "RecipeTag",
                newName: "IX_RecipeTag_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Recipes_RecipeId",
                table: "RecipeTag",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Tags_TagId",
                table: "RecipeTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Recipes_RecipeId",
                table: "RecipeTag");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTag_Tags_TagId",
                table: "RecipeTag");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "RecipeTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipeTag",
                newName: "RecipesId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTag_TagId",
                table: "RecipeTag",
                newName: "IX_RecipeTag_TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Recipes_RecipesId",
                table: "RecipeTag",
                column: "RecipesId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTag_Tags_TagsId",
                table: "RecipeTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
