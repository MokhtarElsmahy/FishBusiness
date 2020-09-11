using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class LeaderLaon_LeaderPayBack_HalakaHalek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HalakaHaleks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    DebtID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalakaHaleks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HalakaHaleks_Debts_DebtID",
                        column: x => x.DebtID,
                        principalTable: "Debts",
                        principalColumn: "DebtID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderLoans",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    BoatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderLoans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaderLoans_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderPaybacks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    BoatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderPaybacks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaderPaybacks_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HalakaHaleks_DebtID",
                table: "HalakaHaleks",
                column: "DebtID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderLoans_BoatID",
                table: "LeaderLoans",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderPaybacks_BoatID",
                table: "LeaderPaybacks",
                column: "BoatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HalakaHaleks");

            migrationBuilder.DropTable(
                name: "LeaderLoans");

            migrationBuilder.DropTable(
                name: "LeaderPaybacks");
        }
    }
}
