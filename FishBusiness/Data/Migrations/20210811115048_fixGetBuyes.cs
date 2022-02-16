using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class fixGetBuyes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoatID",
                table: "IMerchantRecieptItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IMerchantRecieptItem_BoatID",
                table: "IMerchantRecieptItem",
                column: "BoatID");

            migrationBuilder.AddForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem",
                column: "BoatID",
                principalTable: "Boats",
                principalColumn: "BoatID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMerchantRecieptItem_Boats_BoatID",
                table: "IMerchantRecieptItem");

            migrationBuilder.DropIndex(
                name: "IX_IMerchantRecieptItem_BoatID",
                table: "IMerchantRecieptItem");

            migrationBuilder.DropColumn(
                name: "BoatID",
                table: "IMerchantRecieptItem");
        }
    }
}
