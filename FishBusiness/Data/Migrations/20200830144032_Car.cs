using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Car : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarDistination",
                table: "ISellerReciepts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CarPrice",
                table: "ISellerReciepts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarDistination",
                table: "ISellerReciepts");

            migrationBuilder.DropColumn(
                name: "CarPrice",
                table: "ISellerReciepts");
        }
    }
}
