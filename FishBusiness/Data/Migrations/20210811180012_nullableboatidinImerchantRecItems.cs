using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class nullableboatidinImerchantRecItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem");

            migrationBuilder.AlterColumn<int>(
                name: "BoatID",
                table: "IMerchantRecieptItem",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem",
                column: "BoatID",
                principalTable: "Boats",
                principalColumn: "BoatID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem");

            migrationBuilder.AlterColumn<int>(
                name: "BoatID",
                table: "IMerchantRecieptItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem",
                column: "BoatID",
                principalTable: "Boats",
                principalColumn: "BoatID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
