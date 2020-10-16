using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class MerchantID_in_SellerRecs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MerchantID",
                table: "SellerRecs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SellerRecs_MerchantID",
                table: "SellerRecs",
                column: "MerchantID");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerRecs_Merchants_MerchantID",
                table: "SellerRecs",
                column: "MerchantID",
                principalTable: "Merchants",
                principalColumn: "MerchantID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerRecs_Merchants_MerchantID",
                table: "SellerRecs");

            migrationBuilder.DropIndex(
                name: "IX_SellerRecs_MerchantID",
                table: "SellerRecs");

            migrationBuilder.DropColumn(
                name: "MerchantID",
                table: "SellerRecs");
        }
    }
}
