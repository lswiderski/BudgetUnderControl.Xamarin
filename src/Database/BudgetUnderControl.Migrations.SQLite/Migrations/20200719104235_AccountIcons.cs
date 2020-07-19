using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class AccountIcons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconBackgroundColor",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFont",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconGlyph",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconBackgroundColor",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IconColor",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IconFont",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IconGlyph",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Account");
        }
    }
}
