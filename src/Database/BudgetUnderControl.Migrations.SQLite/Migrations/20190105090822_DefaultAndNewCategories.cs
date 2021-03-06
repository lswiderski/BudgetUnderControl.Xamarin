﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class DefaultAndNewCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Category",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("UPDATE [Category] SET [IsDefault] = 1 WHERE [NAME] ='Other'");

            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                                "VALUES ('Beauty',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80cf16-02db-12de-9ee7-5d9e78a695db' )");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                               "VALUES ('Groceries',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80cf16-02db-442a-9ee7-5d9e785df5db')");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                               "VALUES ('Loans',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb23cf16-02db-cd5a-9ee7-5d912ed695db')");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                               "VALUES ('Gift',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80ca16-02db-d2e2-9ee7-5d9e78a695db')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Category");
        }
    }
}
