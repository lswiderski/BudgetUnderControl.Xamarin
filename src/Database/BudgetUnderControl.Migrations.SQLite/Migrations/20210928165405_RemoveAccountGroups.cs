using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class RemoveAccountGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Account_AccountGroup",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_AccountGroupId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccountGroupId",
                table: "Account");

          //  migrationBuilder.DropTable(
          //     name: "AccountGroup"); Cannot drop this table
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "User",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountGroupId",
                table: "Account",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalId = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroup", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_AccountGroup_User",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountGroupId",
                table: "Account",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroup_OwnerId",
                table: "AccountGroup",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Account_AccountGroup",
                table: "Account",
                column: "AccountGroupId",
                principalTable: "AccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
