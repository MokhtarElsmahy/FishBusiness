using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class AdditionalPayment_Collecting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalPayments",
                columns: table => new
                {
                    AdditionalPaymentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    AdditionalPaymentID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalPayments", x => x.AdditionalPaymentID);
                    table.ForeignKey(
                        name: "FK_AdditionalPayments_AdditionalPayments_AdditionalPaymentID1",
                        column: x => x.AdditionalPaymentID1,
                        principalTable: "AdditionalPayments",
                        principalColumn: "AdditionalPaymentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Collectings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPaidFromMerchants = table.Column<decimal>(nullable: false),
                    TotalPaidForMerchants = table.Column<decimal>(nullable: false),
                    TotalHalek = table.Column<decimal>(nullable: false),
                    TotalForFahAllah = table.Column<decimal>(nullable: false),
                    TotalOfAdditionalPayment = table.Column<decimal>(nullable: false),
                    AdditionalPaymentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collectings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Collectings_AdditionalPayments_AdditionalPaymentID",
                        column: x => x.AdditionalPaymentID,
                        principalTable: "AdditionalPayments",
                        principalColumn: "AdditionalPaymentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPayments_AdditionalPaymentID1",
                table: "AdditionalPayments",
                column: "AdditionalPaymentID1");

            migrationBuilder.CreateIndex(
                name: "IX_Collectings_AdditionalPaymentID",
                table: "Collectings",
                column: "AdditionalPaymentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collectings");

            migrationBuilder.DropTable(
                name: "AdditionalPayments");
        }
    }
}
