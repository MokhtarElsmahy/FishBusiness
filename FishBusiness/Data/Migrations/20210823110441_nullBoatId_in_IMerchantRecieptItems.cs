using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class nullBoatId_in_IMerchantRecieptItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantRecieptItems_Boats_BoatID",
                table: "MerchantRecieptItems");

            migrationBuilder.AlterColumn<int>(
                name: "BoatID",
                table: "MerchantRecieptItems",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantRecieptItems_Boats_BoatID",
                table: "MerchantRecieptItems",
                column: "BoatID",
                principalTable: "Boats",
                principalColumn: "BoatID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantRecieptItems_Boats_BoatID",
                table: "MerchantRecieptItems");

            migrationBuilder.AlterColumn<int>(
                name: "BoatID",
                table: "MerchantRecieptItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantRecieptItems_Boats_BoatID",
                table: "MerchantRecieptItems",
                column: "BoatID",
                principalTable: "Boats",
                principalColumn: "BoatID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
