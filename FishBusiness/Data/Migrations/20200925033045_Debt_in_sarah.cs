using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Debt_in_sarah : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Debts_In_Sarhas",
                columns: table => new
                {
                    SarhaID = table.Column<int>(nullable: false),
                    DebtID = table.Column<int>(nullable: false),
                    PersonID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts_In_Sarhas", x => new { x.DebtID, x.SarhaID, x.PersonID });
                    table.ForeignKey(
                        name: "FK_Debts_In_Sarhas_Debts_DebtID",
                        column: x => x.DebtID,
                        principalTable: "Debts",
                        principalColumn: "DebtID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Debts_In_Sarhas_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Debts_In_Sarhas_Sarhas_SarhaID",
                        column: x => x.SarhaID,
                        principalTable: "Sarhas",
                        principalColumn: "SarhaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_In_Sarhas_PersonID",
                table: "Debts_In_Sarhas",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Debts_In_Sarhas_SarhaID",
                table: "Debts_In_Sarhas",
                column: "SarhaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debts_In_Sarhas");
        }
    }
}
