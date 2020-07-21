using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class ChangeIconsSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //SQLite does not support dropcolumn
            /*migrationBuilder.DropColumn(
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
                table: "Account");*/

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Account",
                nullable: true); 

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Utensils.FAS' WHERE [NAME] ='Food'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Bus.FAS' WHERE [NAME] ='Transport'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'StickyNote.FAR' WHERE [NAME] ='Other'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'HandHoldingUsd.FAS' WHERE [NAME] ='Salary'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Receipt.FAS' WHERE [NAME] ='Taxes'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'GlassCheers.FAS' WHERE [NAME] ='Entertainment'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Heartbeat.FAS' WHERE [NAME] ='Health'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'MapMarkerAlt.FAS' WHERE [NAME] ='Interest'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Home.FAS' WHERE [NAME] ='Home'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Smile.FAR' WHERE [NAME] ='Beauty'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'ShoppingBasket.FAS' WHERE [NAME] ='Groceries'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'MoneyCheckAlt.FAS' WHERE [NAME] ='Loans'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Gift.FAS' WHERE [NAME] ='Gift'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Bed.FAS' WHERE [NAME] ='Accommodation'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Landmark.FAS' WHERE [NAME] ='Sightseeing'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Tshirt.FAS' WHERE [NAME] ='Shopping'");

            migrationBuilder.Sql("UPDATE [Category] SET [Icon] = 'Suitcase.FAS' WHERE [NAME] ='Professional Costs'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "IconBackgroundColor",
                table: "Category",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                table: "Category",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFont",
                table: "Category",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconGlyph",
                table: "Category",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconBackgroundColor",
                table: "Account",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconColor",
                table: "Account",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFont",
                table: "Account",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconGlyph",
                table: "Account",
                type: "TEXT",
                nullable: true);
        }
    }
}
