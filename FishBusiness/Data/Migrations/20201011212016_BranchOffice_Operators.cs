using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class BranchOffice_Operators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchOffices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentCredit = table.Column<decimal>(nullable: false),
                    Collecting = table.Column<decimal>(nullable: false),
                    ExternalMerchantTotal = table.Column<decimal>(nullable: false),
                    LocalMerchantsTotal = table.Column<decimal>(nullable: false),
                    OfficeMoney = table.Column<decimal>(nullable: false),
                    ExpensesTotal = table.Column<decimal>(nullable: false),
                    SarhasTotal = table.Column<decimal>(nullable: false),
                    FathallahSalary = table.Column<decimal>(nullable: false),
                    DriversSalary = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchOffices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    OperatorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatorName = table.Column<string>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    JobDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.OperatorID);
                });

            migrationBuilder.CreateTable(
                name: "PaidForOperators",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatorID = table.Column<int>(nullable: false),
                    Payment = table.Column<decimal>(nullable: false),
                    PreviousDebtsForMerchant = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PersonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidForOperators", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaidForOperators_Operators_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Operators",
                        principalColumn: "OperatorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaidForOperators_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaidForOperators_OperatorID",
                table: "PaidForOperators",
                column: "OperatorID");

            migrationBuilder.CreateIndex(
                name: "IX_PaidForOperators_PersonID",
                table: "PaidForOperators",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchOffices");

            migrationBuilder.DropTable(
                name: "PaidForOperators");

            migrationBuilder.DropTable(
                name: "Operators");
        }
    }
}
