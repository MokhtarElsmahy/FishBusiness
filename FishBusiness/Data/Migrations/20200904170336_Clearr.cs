using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class Clearr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");

            //migrationBuilder.DropIndex(
            //    name: "IX_Debts_Sarhas_PersonID",
            //    table: "Debts_Sarhas");

            //migrationBuilder.DropColumn(
            //    name: "PersonID",
            //    table: "Debts_Sarhas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "Debts_Sarhas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonID1 = table.Column<int>(type: "int", nullable: true),
                    credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_Sarhas_PersonID",
                table: "Debts_Sarhas",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_PersonID1",
                table: "Person",
                column: "PersonID1");
        }
    }
}
