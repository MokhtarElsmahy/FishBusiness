using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class AddDateToPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Debts_Sarhas",
                table: "Debts_Sarhas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debts_Sarhas",
                table: "Debts_Sarhas",
                columns: new[] { "DebtID", "SarhaID", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Debts_Sarhas",
                table: "Debts_Sarhas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debts_Sarhas",
                table: "Debts_Sarhas",
                columns: new[] { "DebtID", "SarhaID" });
        }
    }
}
