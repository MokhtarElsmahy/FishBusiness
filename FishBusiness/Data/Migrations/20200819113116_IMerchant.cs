using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class IMerchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDebtsForMerchant",
                table: "Merchants",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "IMerchantReciept",
                columns: table => new
                {
                    IMerchantRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    MerchantID = table.Column<int>(nullable: false),
                    TotalOfReciept = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMerchantReciept", x => x.IMerchantRecieptID);
                    table.ForeignKey(
                        name: "FK_IMerchantReciept_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaidForMerchant",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantID = table.Column<int>(nullable: false),
                    Payment = table.Column<decimal>(nullable: false),
                    PreviousDebtsForMerchant = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsCash = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidForMerchant", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaidForMerchant_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMerchantRecieptItem",
                columns: table => new
                {
                    IMerchantRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false),
                    BoatID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    IMerchantRecieptID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMerchantRecieptItem", x => x.IMerchantRecieptItemID);
                    table.ForeignKey(
                        name: "FK_IMerchantRecieptItem_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IMerchantRecieptItem_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IMerchantRecieptItem_IMerchantReciept_IMerchantRecieptID",
                        column: x => x.IMerchantRecieptID,
                        principalTable: "IMerchantReciept",
                        principalColumn: "IMerchantRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IMerchantRecieptItem_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantReciept_MerchantID",
                table: "IMerchantReciept",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantRecieptItem_BoatID",
                table: "IMerchantRecieptItem",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantRecieptItem_FishID",
                table: "IMerchantRecieptItem",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantRecieptItem_IMerchantRecieptID",
                table: "IMerchantRecieptItem",
                column: "IMerchantRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantRecieptItem_ProductionTypeID",
                table: "IMerchantRecieptItem",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaidForMerchant_MerchantID",
                table: "PaidForMerchant",
                column: "MerchantID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMerchantRecieptItem");

            migrationBuilder.DropTable(
                name: "PaidForMerchant");

            migrationBuilder.DropTable(
                name: "IMerchantReciept");

            migrationBuilder.DropColumn(
                name: "PreviousDebtsForMerchant",
                table: "Merchants");
        }
    }
}
