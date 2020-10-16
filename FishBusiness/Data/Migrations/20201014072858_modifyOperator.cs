using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class modifyOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousDebtsForMerchant",
                table: "PaidForOperators");

            migrationBuilder.AddColumn<decimal>(
                name: "DebtsAfterPayment",
                table: "PaidForOperators",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtsAfterPayment",
                table: "PaidForOperators");

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDebtsForMerchant",
                table: "PaidForOperators",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
