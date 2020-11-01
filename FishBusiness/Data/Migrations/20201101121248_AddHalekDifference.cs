using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class AddHalekDifference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HalekDifferences",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    ReturnedValue = table.Column<decimal>(nullable: false),
                    BoatID = table.Column<int>(nullable: false),
                    PersonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalekDifferences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HalekDifferences_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HalekDifferences_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HalekDifferences_BoatID",
                table: "HalekDifferences",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_HalekDifferences_PersonID",
                table: "HalekDifferences",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HalekDifferences");
        }
    }
}
