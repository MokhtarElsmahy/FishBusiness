using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class addPersonIdTOIsellerReceipts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "ISellerReciepts",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ISellerReciepts_PersonID",
                table: "ISellerReciepts",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_ISellerReciepts_People_PersonID",
                table: "ISellerReciepts",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISellerReciepts_People_PersonID",
                table: "ISellerReciepts");

            migrationBuilder.DropIndex(
                name: "IX_ISellerReciepts_PersonID",
                table: "ISellerReciepts");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "ISellerReciepts");
        }
    }
}
