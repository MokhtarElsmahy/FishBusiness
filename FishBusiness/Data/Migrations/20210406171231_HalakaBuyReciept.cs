using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class HalakaBuyReciept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HalakaBuyReciepts",
                columns: table => new
                {
                    HalakaBuyRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    SellerName = table.Column<string>(nullable: true),
                    TotalOfPrices = table.Column<decimal>(nullable: false),
                    PersonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalakaBuyReciepts", x => x.HalakaBuyRecieptID);
                    table.ForeignKey(
                        name: "FK_HalakaBuyReciepts_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HalakaBuyRecieptItems",
                columns: table => new
                {
                    HalakaBuyRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HalakaBuyRecieptID = table.Column<int>(nullable: false),
                    AmountId = table.Column<Guid>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    BoxQty = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalakaBuyRecieptItems", x => x.HalakaBuyRecieptItemID);
                    table.ForeignKey(
                        name: "FK_HalakaBuyRecieptItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HalakaBuyRecieptItems_HalakaBuyReciepts_HalakaBuyRecieptID",
                        column: x => x.HalakaBuyRecieptID,
                        principalTable: "HalakaBuyReciepts",
                        principalColumn: "HalakaBuyRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HalakaBuyRecieptItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HalakaBuyRecieptItems_FishID",
                table: "HalakaBuyRecieptItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakaBuyRecieptItems_HalakaBuyRecieptID",
                table: "HalakaBuyRecieptItems",
                column: "HalakaBuyRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakaBuyRecieptItems_ProductionTypeID",
                table: "HalakaBuyRecieptItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_HalakaBuyReciepts_PersonID",
                table: "HalakaBuyReciepts",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HalakaBuyRecieptItems");

            migrationBuilder.DropTable(
                name: "HalakaBuyReciepts");
        }
    }
}
