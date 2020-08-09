using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class IncomesOfSharedBoatsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomesOfSharedBoats",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Income = table.Column<decimal>(nullable: false),
                    BoatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomesOfSharedBoats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomesOfSharedBoats_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomesOfSharedBoats_BoatID",
                table: "IncomesOfSharedBoats",
                column: "BoatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomesOfSharedBoats");
        }
    }
}
