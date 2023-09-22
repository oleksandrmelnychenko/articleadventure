using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations.ArticleAdventureData
{
    public partial class updateMainArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "LinkFacebook",
            //    table: "UserProfile",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "LinkInstagram",
            //    table: "UserProfile",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "LinkTelegram",
            //    table: "UserProfile",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "LinkTwitter",
            //    table: "UserProfile",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MainArticle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            //migrationBuilder.AddColumn<long>(
            //    name: "UserProfileId",
            //    table: "MainArticle",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MainArticle_UserProfileId",
            //    table: "MainArticle",
            //    column: "UserProfileId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MainArticle_UserProfile_UserProfileId",
            //    table: "MainArticle",
            //    column: "UserProfileId",
            //    principalTable: "UserProfile",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MainArticle_UserProfile_UserProfileId",
            //    table: "MainArticle");

            //migrationBuilder.DropIndex(
            //    name: "IX_MainArticle_UserProfileId",
            //    table: "MainArticle");

            //migrationBuilder.DropColumn(
            //    name: "LinkFacebook",
            //    table: "UserProfile");

            //migrationBuilder.DropColumn(
            //    name: "LinkInstagram",
            //    table: "UserProfile");

            //migrationBuilder.DropColumn(
            //    name: "LinkTelegram",
            //    table: "UserProfile");

            //migrationBuilder.DropColumn(
            //    name: "LinkTwitter",
            //    table: "UserProfile");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "MainArticle");

            //migrationBuilder.DropColumn(
            //    name: "UserProfileId",
            //    table: "MainArticle");
        }
    }
}
