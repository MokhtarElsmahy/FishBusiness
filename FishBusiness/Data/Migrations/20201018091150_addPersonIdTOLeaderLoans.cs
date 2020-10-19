using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class addPersonIdTOLeaderLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "LeaderLoans",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeaderLoans_PersonID",
                table: "LeaderLoans",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderLoans_People_PersonID",
                table: "LeaderLoans",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderLoans_People_PersonID",
                table: "LeaderLoans");

            migrationBuilder.DropIndex(
                name: "IX_LeaderLoans_PersonID",
                table: "LeaderLoans");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "LeaderLoans");
        }
    }
}
