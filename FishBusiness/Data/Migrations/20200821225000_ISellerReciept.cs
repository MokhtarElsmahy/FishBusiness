using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class ISellerReciept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMerchantRecID",
                table: "Stocks");

            migrationBuilder.CreateTable(
                name: "ISellerReciepts",
                columns: table => new
                {
                    ISellerRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    MerchantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISellerReciepts", x => x.ISellerRecieptID);
                    table.ForeignKey(
                        name: "FK_ISellerReciepts_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ISellerRecieptItems",
                columns: table => new
                {
                    ISellerRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISellerRecieptID = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    BoxQty = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISellerRecieptItems", x => x.ISellerRecieptItemID);
                    table.ForeignKey(
                        name: "FK_ISellerRecieptItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ISellerRecieptItems_ISellerReciepts_ISellerRecieptID",
                        column: x => x.ISellerRecieptID,
                        principalTable: "ISellerReciepts",
                        principalColumn: "ISellerRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ISellerRecieptItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ISellerRecieptItems_FishID",
                table: "ISellerRecieptItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_ISellerRecieptItems_ISellerRecieptID",
                table: "ISellerRecieptItems",
                column: "ISellerRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_ISellerRecieptItems_ProductionTypeID",
                table: "ISellerRecieptItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ISellerReciepts_MerchantID",
                table: "ISellerReciepts",
                column: "MerchantID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ISellerRecieptItems");

            migrationBuilder.DropTable(
                name: "ISellerReciepts");

            migrationBuilder.AddColumn<int>(
                name: "IMerchantRecID",
                table: "Stocks",
                type: "int",
                nullable: true);
        }
    }
}
