using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class amountId_in_MerchantRecItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AmountId",
                table: "MerchantRecieptItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountId",
                table: "MerchantRecieptItems");
        }
    }
}
