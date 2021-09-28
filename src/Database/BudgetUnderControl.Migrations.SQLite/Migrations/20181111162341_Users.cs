using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Transaction",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Tag",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Tag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Tag",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Category",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "AccountGroup",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "AccountGroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "AccountGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Account",
                nullable: false,
                defaultValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Account",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(maxLength: 50, nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AddedById",
                table: "Transaction",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_OwnerId",
                table: "Tag",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_OwnerId",
                table: "Category",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroup_OwnerId",
                table: "AccountGroup",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_OwnerId",
                table: "Account",
                column: "OwnerId");
    /*
            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Account_User",
                table: "Account",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_AccountGroup_User",
                table: "AccountGroup",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_OwnerId",
                table: "Category",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_User_OwnerId",
                table: "Tag",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Transaction_User",
                table: "Transaction",
                column: "AddedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            */
            migrationBuilder.Sql("INSERT INTO USER (Username,Role,Email,Password,Salt,CreatedAt,ExternalId)" +
                                                "VALUES ('user', 'User', 'user@buc.xyz','11a78a6a43d03d06489bf0611735acd0', 's0mRIdlKvI','2018-11-11 15:29:09.584149','10000000-0000-0000-0000-000000000000')"); //password:asdfg
            migrationBuilder.Sql("UPDATE [Transaction] SET [AddedById] = 1");
            migrationBuilder.Sql("UPDATE [Tag] SET [OwnerId] = 1");
            migrationBuilder.Sql("UPDATE [Category] SET [OwnerId] = 1");
            migrationBuilder.Sql("UPDATE [AccountGroup] SET [OwnerId] = 1");
            migrationBuilder.Sql("UPDATE [Account] SET [OwnerId] = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Account_User",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_AccountGroup_User",
                table: "AccountGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_OwnerId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_User_OwnerId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Transaction_User",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_AddedById",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Tag_OwnerId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Category_OwnerId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_AccountGroup_OwnerId",
                table: "AccountGroup");

            migrationBuilder.DropIndex(
                name: "IX_Account_OwnerId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "AccountGroup");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "AccountGroup");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "AccountGroup");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Account");
        }
    }
}
