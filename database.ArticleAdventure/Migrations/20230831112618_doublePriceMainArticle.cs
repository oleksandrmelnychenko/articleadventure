using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class doublePriceMainArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "MainArticle",
                type: "float",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 250);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "MainArticle",
                type: "int",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 250);
        }
    }
}
