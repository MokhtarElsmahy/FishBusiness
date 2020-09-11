using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Commision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidForUs",
                table: "PaidForMerchant",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Commision",
                table: "ISellerReciepts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidForUs",
                table: "PaidForMerchant");

            migrationBuilder.DropColumn(
                name: "Commision",
                table: "ISellerReciepts");
        }
    }
}
