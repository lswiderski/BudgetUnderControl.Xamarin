using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class DeleteUserPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "TEXT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "Username",
               table: "User",
               type: "TEXT",
               maxLength: 50,
               nullable: true);
        }
    }
}
