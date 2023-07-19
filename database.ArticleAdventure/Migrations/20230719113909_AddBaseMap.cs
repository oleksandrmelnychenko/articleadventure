using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.ArticleAdventure.Migrations
{
    public partial class AddBaseMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Blogs_BlogsId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "NetUid",
                table: "Blogs",
                newName: "NetUID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Blogs",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "NetUid",
                table: "Tags",
                newName: "NetUID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tags",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_BlogsId",
                table: "Tags",
                newName: "IX_Tags_BlogsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetUID",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetUID",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Tags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Blogs_BlogsId",
                table: "Tags",
                column: "BlogsId",
                principalTable: "Blogs",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Blogs_BlogsId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameColumn(
                name: "NetUID",
                table: "Blogs",
                newName: "NetUid");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Blogs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NetUID",
                table: "Tag",
                newName: "NetUid");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tag",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_BlogsId",
                table: "Tag",
                newName: "IX_Tag_BlogsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetUid",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Blogs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetUid",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Tag",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Tag",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Blogs_BlogsId",
                table: "Tag",
                column: "BlogsId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }
    }
}
