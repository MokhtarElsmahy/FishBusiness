using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class AdditionalForOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalMerchantTotal",
                table: "BranchOffices");

            migrationBuilder.DropColumn(
                name: "LocalMerchantsTotal",
                table: "BranchOffices");

            migrationBuilder.AddColumn<decimal>(
                name: "IncomeTotal",
                table: "BranchOffices",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AdditionalForOffices",
                columns: table => new
                {
                    AdditionalForOfficeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    PersonID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalForOffices", x => x.AdditionalForOfficeID);
                    table.ForeignKey(
                        name: "FK_AdditionalForOffices_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalForOffices_PersonID",
                table: "AdditionalForOffices",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalForOffices");

            migrationBuilder.DropColumn(
                name: "IncomeTotal",
                table: "BranchOffices");

            migrationBuilder.AddColumn<decimal>(
                name: "ExternalMerchantTotal",
                table: "BranchOffices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LocalMerchantsTotal",
                table: "BranchOffices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
