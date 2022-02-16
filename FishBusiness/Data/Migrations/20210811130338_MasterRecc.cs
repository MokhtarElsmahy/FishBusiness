using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class MasterRecc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterRecieptID",
                table: "BoatOwnerItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MasterReciepts",
                columns: table => new
                {
                    MasterRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: true),
                    SarhaID = table.Column<int>(nullable: true),
                    TotalBeforePaying = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<decimal>(nullable: false),
                    PercentageCommission = table.Column<int>(nullable: false),
                    PaidFromDebts = table.Column<decimal>(nullable: false),
                    TotalAfterPaying = table.Column<decimal>(nullable: false),
                    FinalIncome = table.Column<decimal>(nullable: false),
                    IsCalculated = table.Column<bool>(nullable: false),
                    IsCollected = table.Column<bool>(nullable: false),
                    IsCheckedOut = table.Column<bool>(nullable: false),
                    PersonID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReciepts", x => x.MasterRecieptID);
                    table.ForeignKey(
                        name: "FK_MasterReciepts_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterReciepts_PersonID",
                table: "MasterReciepts",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterReciepts");

            migrationBuilder.DropColumn(
                name: "MasterRecieptID",
                table: "BoatOwnerItems");
        }
    }
}
