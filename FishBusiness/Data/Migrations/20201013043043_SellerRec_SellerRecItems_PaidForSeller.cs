using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class SellerRec_SellerRecItems_PaidForSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaidForSellers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Payment = table.Column<decimal>(nullable: false),
                    PreviousDebtsForSeller = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsCash = table.Column<bool>(nullable: false),
                    PersonID = table.Column<int>(nullable: false),
                    MerchantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidForSellers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaidForSellers_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaidForSellers_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerRecs",
                columns: table => new
                {
                    SellerRecID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalBeforePaying = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<decimal>(nullable: false),
                    PercentageCommission = table.Column<int>(nullable: false),
                    FinalIncome = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerRecs", x => x.SellerRecID);
                });

            migrationBuilder.CreateTable(
                name: "SellerRecItems",
                columns: table => new
                {
                    SellerRecItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerRectID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AmountId = table.Column<Guid>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerRecItems", x => x.SellerRecItemID);
                    table.ForeignKey(
                        name: "FK_SellerRecItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerRecItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerRecItems_SellerRecs_SellerRectID",
                        column: x => x.SellerRectID,
                        principalTable: "SellerRecs",
                        principalColumn: "SellerRecID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaidForSellers_MerchantID",
                table: "PaidForSellers",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_PaidForSellers_PersonID",
                table: "PaidForSellers",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_SellerRecItems_FishID",
                table: "SellerRecItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_SellerRecItems_ProductionTypeID",
                table: "SellerRecItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SellerRecItems_SellerRectID",
                table: "SellerRecItems",
                column: "SellerRectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaidForSellers");

            migrationBuilder.DropTable(
                name: "SellerRecItems");

            migrationBuilder.DropTable(
                name: "SellerRecs");
        }
    }
}
