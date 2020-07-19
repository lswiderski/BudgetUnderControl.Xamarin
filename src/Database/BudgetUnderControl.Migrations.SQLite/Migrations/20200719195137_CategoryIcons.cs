using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class CategoryIcons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                   "VALUES ('Accommodation',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80ca16-02db-d2e2-9ee7-5d9a78a695db')");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                   "VALUES ('Sightseeing',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80ca16-02db-d2e2-9ee7-5d4f78a695db')");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                   "VALUES ('Shopping',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80ca16-02db-d2e2-9ee7-5d3b78a695db')");
            migrationBuilder.Sql("INSERT INTO CATEGORY (Name,OwnerId,ModifiedOn,ExternalId)" +
                                   "VALUES ('Professional Costs',(SELECT Id FROM [USER] LIMIT 1),DATETIME('now'),'cb80ca16-02db-d2e2-9ee7-5d2d78a695db')");

            migrationBuilder.AddColumn<string>(
                name: "IconBackgroundColor",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFont",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconGlyph",
                table: "Category",
                nullable: true);

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Food'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf2e7' WHERE [NAME] ='Food'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Transport'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf207' WHERE [NAME] ='Transport'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAR' WHERE [NAME] ='Other'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf249' WHERE [NAME] ='Other'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Salary'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf4c0' WHERE [NAME] ='Salary'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Taxes'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf543' WHERE [NAME] ='Taxes'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Entertainment'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf79f' WHERE [NAME] ='Entertainment'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Health'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf21e' WHERE [NAME] ='Health'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Interest'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf3c5' WHERE [NAME] ='Interest'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Home'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf015' WHERE [NAME] ='Home'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAR' WHERE [NAME] ='Beauty'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf118' WHERE [NAME] ='Beauty'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Groceries'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf291' WHERE [NAME] ='Groceries'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Loans'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf53d' WHERE [NAME] ='Loans'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Gift'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf06b' WHERE [NAME] ='Gift'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Accommodation'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf236' WHERE [NAME] ='Accommodation'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Sightseeing'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf66f' WHERE [NAME] ='Sightseeing'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Shopping'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf553' WHERE [NAME] ='Shopping'");

            migrationBuilder.Sql("UPDATE [Category] SET [IconFont] = 'FAS' WHERE [NAME] ='Professional Costs'");
            migrationBuilder.Sql("UPDATE [Category] SET [IconGlyph] = '\uf0f2' WHERE [NAME] ='Professional Costs'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconBackgroundColor",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IconColor",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IconFont",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IconGlyph",
                table: "Category");
        }
    }
}
