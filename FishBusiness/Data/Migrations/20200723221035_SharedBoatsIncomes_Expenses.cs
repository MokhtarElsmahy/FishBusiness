using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class SharedBoatsIncomes_Expenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtsOfMulfunction",
                table: "Boats");

            migrationBuilder.AddColumn<decimal>(
                name: "IncomeOfSharedBoat",
                table: "Boats",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalIncome",
                table: "BoatOwnerReciepts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Cause = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseID);
                    table.ForeignKey(
                        name: "FK_Expenses_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedBoatsIncomes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: false),
                    Income = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedBoatsIncomes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SharedBoatsIncomes_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BoatID",
                table: "Expenses",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_SharedBoatsIncomes_BoatID",
                table: "SharedBoatsIncomes",
                column: "BoatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "SharedBoatsIncomes");

            migrationBuilder.DropColumn(
                name: "IncomeOfSharedBoat",
                table: "Boats");

            migrationBuilder.DropColumn(
                name: "FinalIncome",
                table: "BoatOwnerReciepts");

            migrationBuilder.AddColumn<decimal>(
                name: "DebtsOfMulfunction",
                table: "Boats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
