using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class halakaSellReciepts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HalakSellReciepts",
                columns: table => new
                {
                    HalakSellRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    buyerName = table.Column<string>(nullable: true),
                    TotalOfPrices = table.Column<double>(nullable: false),
                    PersonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalakSellReciepts", x => x.HalakSellRecieptID);
                    table.ForeignKey(
                        name: "FK_HalakSellReciepts_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HalakSellRecieptItems",
                columns: table => new
                {
                    HalakSellRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HalakSellRecieptID = table.Column<int>(nullable: false),
                    Qty = table.Column<double>(nullable: false),
                    BoxQty = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalakSellRecieptItems", x => x.HalakSellRecieptItemID);
                    table.ForeignKey(
                        name: "FK_HalakSellRecieptItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HalakSellRecieptItems_HalakSellReciepts_HalakSellRecieptID",
                        column: x => x.HalakSellRecieptID,
                        principalTable: "HalakSellReciepts",
                        principalColumn: "HalakSellRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HalakSellRecieptItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HalakSellRecieptItems_FishID",
                table: "HalakSellRecieptItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakSellRecieptItems_HalakSellRecieptID",
                table: "HalakSellRecieptItems",
                column: "HalakSellRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakSellRecieptItems_ProductionTypeID",
                table: "HalakSellRecieptItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakSellReciepts_PersonID",
                table: "HalakSellReciepts",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HalakSellRecieptItems");

            migrationBuilder.DropTable(
                name: "HalakSellReciepts");
        }
    }
}
