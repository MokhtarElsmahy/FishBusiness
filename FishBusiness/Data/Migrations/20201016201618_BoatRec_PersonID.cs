using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class BoatRec_PersonID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "BoatOwnerReciepts",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerReciepts_PersonID",
                table: "BoatOwnerReciepts",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_BoatOwnerReciepts_People_PersonID",
                table: "BoatOwnerReciepts",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoatOwnerReciepts_People_PersonID",
                table: "BoatOwnerReciepts");

            migrationBuilder.DropIndex(
                name: "IX_BoatOwnerReciepts_PersonID",
                table: "BoatOwnerReciepts");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "BoatOwnerReciepts");
        }
    }
}
