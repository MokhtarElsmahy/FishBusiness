using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class dropping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Debts_Sarhas_Manager_ManagerID",
            //    table: "Debts_Sarhas");

            migrationBuilder.DropTable(
                name: "Manager");

            //migrationBuilder.DropIndex(
            //    name: "IX_Debts_Sarhas_ManagerID",
            //    table: "Debts_Sarhas");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "Debts_Sarhas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerID",
                table: "Debts_Sarhas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_Sarhas_ManagerID",
                table: "Debts_Sarhas",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Sarhas_Manager_ManagerID",
                table: "Debts_Sarhas",
                column: "ManagerID",
                principalTable: "Manager",
                principalColumn: "ManagerID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
