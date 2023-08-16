using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class AddTablesArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainArticleMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InfromationArticle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    NetUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainArticleMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainTags",
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
                    table.PrimaryKey("PK_MainTags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentAmount = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuthorArticle",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainArticleId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WebImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EditorValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorArticle", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AuthorArticle_MainArticleMaps_MainArticleId",
                        column: x => x.MainArticleId,
                        principalTable: "MainArticleMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubTags",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Color = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdMainTag = table.Column<long>(type: "bigint", nullable: false),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubTags_MainTags_IdMainTag",
                        column: x => x.IdMainTag,
                        principalTable: "MainTags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainArticleId = table.Column<long>(type: "bigint", nullable: false),
                    SupTagId = table.Column<long>(type: "bigint", nullable: false),
                    NetUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleTags_MainArticleMaps_MainArticleId",
                        column: x => x.MainArticleId,
                        principalTable: "MainArticleMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_SubTags_SupTagId",
                        column: x => x.SupTagId,
                        principalTable: "SubTags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_MainArticleId",
                table: "ArticleTags",
                column: "MainArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_SupTagId",
                table: "ArticleTags",
                column: "SupTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorArticle_MainArticleId",
                table: "AuthorArticle",
                column: "MainArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTags_IdMainTag",
                table: "SubTags",
                column: "IdMainTag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "AuthorArticle");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "SubTags");

            migrationBuilder.DropTable(
                name: "MainArticleMaps");

            migrationBuilder.DropTable(
                name: "MainTags");
        }
    }
}
