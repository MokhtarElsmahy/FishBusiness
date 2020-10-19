using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class addPaidForBoats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaidForBoats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Payment = table.Column<decimal>(nullable: false),
                    HalekDebtsTillNow = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PersonID = table.Column<int>(nullable: false),
                    BoatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidForBoats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaidForBoats_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaidForBoats_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaidForBoats_BoatID",
                table: "PaidForBoats",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_PaidForBoats_PersonID",
                table: "PaidForBoats",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaidForBoats");
        }
    }
}
