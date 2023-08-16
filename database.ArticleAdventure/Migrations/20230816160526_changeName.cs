using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class changeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTags_MainArticleMaps_MainArticleId",
                table: "ArticleTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorArticle_MainArticleMaps_MainArticleId",
                table: "AuthorArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainArticleMaps",
                table: "MainArticleMaps");

            migrationBuilder.RenameTable(
                name: "MainArticleMaps",
                newName: "MainArticle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainArticle",
                table: "MainArticle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTags_MainArticle_MainArticleId",
                table: "ArticleTags",
                column: "MainArticleId",
                principalTable: "MainArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorArticle_MainArticle_MainArticleId",
                table: "AuthorArticle",
                column: "MainArticleId",
                principalTable: "MainArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTags_MainArticle_MainArticleId",
                table: "ArticleTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorArticle_MainArticle_MainArticleId",
                table: "AuthorArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainArticle",
                table: "MainArticle");

            migrationBuilder.RenameTable(
                name: "MainArticle",
                newName: "MainArticleMaps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainArticleMaps",
                table: "MainArticleMaps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTags_MainArticleMaps_MainArticleId",
                table: "ArticleTags",
                column: "MainArticleId",
                principalTable: "MainArticleMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorArticle_MainArticleMaps_MainArticleId",
                table: "AuthorArticle",
                column: "MainArticleId",
                principalTable: "MainArticleMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
