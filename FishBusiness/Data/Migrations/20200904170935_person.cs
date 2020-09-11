using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "Debts_Sarhas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    credit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_Sarhas_PersonID",
                table: "Debts_Sarhas",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Sarhas_People_PersonID",
                table: "Debts_Sarhas",
                column: "PersonID",
                principalTable: "People",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Sarhas_People_PersonID",
                table: "Debts_Sarhas");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropIndex(
                name: "IX_Debts_Sarhas_PersonID",
                table: "Debts_Sarhas");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "Debts_Sarhas");
        }
    }
}
