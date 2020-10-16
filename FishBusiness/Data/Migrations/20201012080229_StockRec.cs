using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class StockRec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockRecs",
                columns: table => new
                {
                    StockRecID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    TotalOfRec = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockRecs", x => x.StockRecID);
                });

            migrationBuilder.CreateTable(
                name: "StockRecItems",
                columns: table => new
                {
                    StockRecItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockRecID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AmountId = table.Column<Guid>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockRecItems", x => x.StockRecItemID);
                    table.ForeignKey(
                        name: "FK_StockRecItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockRecItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockRecItems_StockRecs_StockRecID",
                        column: x => x.StockRecID,
                        principalTable: "StockRecs",
                        principalColumn: "StockRecID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockRecItems_FishID",
                table: "StockRecItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_StockRecItems_ProductionTypeID",
                table: "StockRecItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_StockRecItems_StockRecID",
                table: "StockRecItems",
                column: "StockRecID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockRecItems");

            migrationBuilder.DropTable(
                name: "StockRecs");
        }
    }
}
