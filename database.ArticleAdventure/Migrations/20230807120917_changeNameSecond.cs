using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class changeNameSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Blogs_AuthorArticleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_mainTags_IdMainTag",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mainTags",
                table: "mainTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "mainTags",
                newName: "MainTags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "SubTags");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_IdMainTag",
                table: "SubTags",
                newName: "IX_SubTags_IdMainTag");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_AuthorArticleId",
                table: "SubTags",
                newName: "IX_SubTags_AuthorArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainTags",
                table: "MainTags",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTags",
                table: "SubTags",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTags_Blogs_AuthorArticleId",
                table: "SubTags",
                column: "AuthorArticleId",
                principalTable: "Blogs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTags_MainTags_IdMainTag",
                table: "SubTags",
                column: "IdMainTag",
                principalTable: "MainTags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTags_Blogs_AuthorArticleId",
                table: "SubTags");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTags_MainTags_IdMainTag",
                table: "SubTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainTags",
                table: "MainTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTags",
                table: "SubTags");

            migrationBuilder.RenameTable(
                name: "MainTags",
                newName: "mainTags");

            migrationBuilder.RenameTable(
                name: "SubTags",
                newName: "Tags");

            migrationBuilder.RenameIndex(
                name: "IX_SubTags_IdMainTag",
                table: "Tags",
                newName: "IX_Tags_IdMainTag");

            migrationBuilder.RenameIndex(
                name: "IX_SubTags_AuthorArticleId",
                table: "Tags",
                newName: "IX_Tags_AuthorArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mainTags",
                table: "mainTags",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Blogs_AuthorArticleId",
                table: "Tags",
                column: "AuthorArticleId",
                principalTable: "Blogs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_mainTags_IdMainTag",
                table: "Tags",
                column: "IdMainTag",
                principalTable: "mainTags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
