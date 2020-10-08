using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class AddPersonId_TOPaidForMerchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "PaidForMerchant",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaidForMerchant_PersonID",
                table: "PaidForMerchant",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidForMerchant_People_PersonID",
                table: "PaidForMerchant",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidForMerchant_People_PersonID",
                table: "PaidForMerchant");

            migrationBuilder.DropIndex(
                name: "IX_PaidForMerchant_PersonID",
                table: "PaidForMerchant");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "PaidForMerchant");
        }
    }
}
