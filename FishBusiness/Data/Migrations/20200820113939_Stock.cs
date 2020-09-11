using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishID = table.Column<int>(nullable: false),
                    Qty = table.Column<double>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockID);
                    table.ForeignKey(
                        name: "FK_Stocks_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_FishID",
                table: "Stocks",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductionTypeID",
                table: "Stocks",
                column: "ProductionTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
