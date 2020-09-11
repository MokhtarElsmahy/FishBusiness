using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class colllecting_additional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalPayments_AdditionalPayments_AdditionalPaymentID1",
                table: "AdditionalPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Collectings_AdditionalPayments_AdditionalPaymentID",
                table: "Collectings");

            migrationBuilder.DropIndex(
                name: "IX_Collectings_AdditionalPaymentID",
                table: "Collectings");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalPayments_AdditionalPaymentID1",
                table: "AdditionalPayments");

            migrationBuilder.DropColumn(
                name: "AdditionalPaymentID",
                table: "Collectings");

            migrationBuilder.DropColumn(
                name: "AdditionalPaymentID1",
                table: "AdditionalPayments");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "AdditionalPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPayments_ID",
                table: "AdditionalPayments",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalPayments_Collectings_ID",
                table: "AdditionalPayments",
                column: "ID",
                principalTable: "Collectings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalPayments_Collectings_ID",
                table: "AdditionalPayments");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalPayments_ID",
                table: "AdditionalPayments");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "AdditionalPayments");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalPaymentID",
                table: "Collectings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdditionalPaymentID1",
                table: "AdditionalPayments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collectings_AdditionalPaymentID",
                table: "Collectings",
                column: "AdditionalPaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPayments_AdditionalPaymentID1",
                table: "AdditionalPayments",
                column: "AdditionalPaymentID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalPayments_AdditionalPayments_AdditionalPaymentID1",
                table: "AdditionalPayments",
                column: "AdditionalPaymentID1",
                principalTable: "AdditionalPayments",
                principalColumn: "AdditionalPaymentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Collectings_AdditionalPayments_AdditionalPaymentID",
                table: "Collectings",
                column: "AdditionalPaymentID",
                principalTable: "AdditionalPayments",
                principalColumn: "AdditionalPaymentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
