using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class updateUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkFacebook",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkInstagram",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkTelegram",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkTwitter",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkFacebook",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "LinkInstagram",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "LinkTelegram",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "LinkTwitter",
                table: "UserProfile");
        }
    }
}
