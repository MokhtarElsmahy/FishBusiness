using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class fixx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterRecieptID",
                table: "BoatOwnerItems");

            migrationBuilder.AddColumn<int>(
                name: "MasterRecieptID",
                table: "BoatOwnerReciepts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterRecieptID",
                table: "BoatOwnerReciepts");

            migrationBuilder.AddColumn<int>(
                name: "MasterRecieptID",
                table: "BoatOwnerItems",
                type: "int",
                nullable: true);
        }
    }
}
