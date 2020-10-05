using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class Goals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    UntilDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConnectiveOfConditions = table.Column<byte>(nullable: false),
                    ValueIfFail = table.Column<decimal>(nullable: true),
                    Repeat = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsReached = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goal", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_Goal_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoalCondition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GoalId = table.Column<int>(nullable: false),
                    GoalExternalId = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    AccountExternalId = table.Column<string>(nullable: true),
                    ConditionType = table.Column<byte>(nullable: false),
                    Value = table.Column<decimal>(nullable: true),
                    TagId = table.Column<int>(nullable: true),
                    TagExternalId = table.Column<string>(nullable: true),
                    Until = table.Column<int>(nullable: false),
                    UntilTimePeriod = table.Column<byte>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    UntilDate = table.Column<DateTime>(nullable: false),
                    IsPercent = table.Column<bool>(nullable: false),
                    TransactionType = table.Column<byte>(nullable: false),
                    Comparer = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalCondition", x => x.Id);
                    table.ForeignKey(
                        name: "ForeignKey_GoalCondition_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_GoalCondition_Goal",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_GoalCondition_Tag",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goal_UserId",
                table: "Goal",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalCondition_AccountId",
                table: "GoalCondition",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalCondition_GoalId",
                table: "GoalCondition",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalCondition_TagId",
                table: "GoalCondition",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalCondition");

            migrationBuilder.DropTable(
                name: "Goal");
        }
    }
}
