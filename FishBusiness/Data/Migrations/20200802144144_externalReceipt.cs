using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class externalReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalReceipts",
                columns: table => new
                {
                    ExternalReceiptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: false),
                    SarhaID = table.Column<int>(nullable: false),
                    TotalBeforePaying = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<decimal>(nullable: false),
                    PaidFromDebts = table.Column<decimal>(nullable: false),
                    TotalAfterPaying = table.Column<decimal>(nullable: false),
                    FinalIncome = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalReceipts", x => x.ExternalReceiptID);
                    table.ForeignKey(
                        name: "FK_ExternalReceipts_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExternalReceipts_Sarhas_SarhaID",
                        column: x => x.SarhaID,
                        principalTable: "Sarhas",
                        principalColumn: "SarhaID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalReceipts_BoatID",
                table: "ExternalReceipts",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalReceipts_SarhaID",
                table: "ExternalReceipts",
                column: "SarhaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalReceipts");
        }
    }
}
