using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class UpdateFavoriteArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MainArticle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
            migrationBuilder.CreateTable(
                name: "FavoriteArticles",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: false),
                    MainArticleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainArticleId1 = table.Column<long>(type: "bigint", nullable: false),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteArticles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FavoriteArticles_MainArticle_MainArticleId1",
                        column: x => x.MainArticleId1,
                        principalTable: "MainArticle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteArticles_UserProfile_UserId1",
                        column: x => x.UserId1,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteArticles_MainArticleId1",
                table: "FavoriteArticles",
                column: "MainArticleId1");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteArticles_UserId1",
                table: "FavoriteArticles",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteArticles");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MainArticle",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
