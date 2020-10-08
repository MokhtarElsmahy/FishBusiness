using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class PersonReciept_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonReciepts",
                columns: table => new
                {
                    PersonRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonName = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonReciepts", x => x.PersonRecieptID);
                });

            migrationBuilder.CreateTable(
                name: "PersonRecieptItems",
                columns: table => new
                {
                    PersonRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false),
                    BoatID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    PersonRecieptID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRecieptItems", x => x.PersonRecieptItemID);
                    table.ForeignKey(
                        name: "FK_PersonRecieptItems_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRecieptItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRecieptItems_PersonReciepts_PersonRecieptID",
                        column: x => x.PersonRecieptID,
                        principalTable: "PersonReciepts",
                        principalColumn: "PersonRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRecieptItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonRecieptItems_BoatID",
                table: "PersonRecieptItems",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRecieptItems_FishID",
                table: "PersonRecieptItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRecieptItems_PersonRecieptID",
                table: "PersonRecieptItems",
                column: "PersonRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRecieptItems_ProductionTypeID",
                table: "PersonRecieptItems",
                column: "ProductionTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonRecieptItems");

            migrationBuilder.DropTable(
                name: "PersonReciepts");
        }
    }
}
