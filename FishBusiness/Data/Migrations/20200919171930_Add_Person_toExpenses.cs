using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Add_Person_toExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "Expenses",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PersonID",
                table: "Expenses",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_People_PersonID",
                table: "Expenses",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_People_PersonID",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PersonID",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "Expenses");
        }
    }
}
