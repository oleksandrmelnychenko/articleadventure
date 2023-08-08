using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Blogs_BlogsId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "BlogsId",
                table: "Tags",
                newName: "AuthorArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_BlogsId",
                table: "Tags",
                newName: "IX_Tags_AuthorArticleId");

            migrationBuilder.AddColumn<long>(
                name: "IdMainTag",
                table: "Tags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "mainTags",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mainTags", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_IdMainTag",
                table: "Tags",
                column: "IdMainTag");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Blogs_AuthorArticleId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_mainTags_IdMainTag",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "mainTags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_IdMainTag",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IdMainTag",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "AuthorArticleId",
                table: "Tags",
                newName: "BlogsId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_AuthorArticleId",
                table: "Tags",
                newName: "IX_Tags_BlogsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Blogs_BlogsId",
                table: "Tags",
                column: "BlogsId",
                principalTable: "Blogs",
                principalColumn: "ID");
        }
    }
}
