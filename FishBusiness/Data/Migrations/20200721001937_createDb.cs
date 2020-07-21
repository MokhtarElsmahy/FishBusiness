using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishBusiness.Data.Migrations
{
    public partial class createDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoatTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoatTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Cofigs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cofigs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Debts",
                columns: table => new
                {
                    DebtID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DebtName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts", x => x.DebtID);
                });

            migrationBuilder.CreateTable(
                name: "Fishes",
                columns: table => new
                {
                    FishID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishes", x => x.FishID);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantName = table.Column<string>(nullable: false),
                    PreviousDebts = table.Column<decimal>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantID);
                });

            migrationBuilder.CreateTable(
                name: "ProductionTypes",
                columns: table => new
                {
                    ProductionTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionTypes", x => x.ProductionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Boats",
                columns: table => new
                {
                    BoatID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatName = table.Column<string>(nullable: false),
                    BoatLeader = table.Column<string>(nullable: false),
                    DebtsOfHalek = table.Column<decimal>(nullable: false),
                    DebtsOfMulfunction = table.Column<decimal>(nullable: false),
                    DebtsOfStartingWork = table.Column<decimal>(nullable: false),
                    BoatLicenseNumber = table.Column<string>(nullable: false),
                    BoatNumber = table.Column<string>(nullable: false),
                    BoatImage = table.Column<string>(nullable: true),
                    TypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boats", x => x.BoatID);
                    table.ForeignKey(
                        name: "FK_Boats_BoatTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "BoatTypes",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantReciepts",
                columns: table => new
                {
                    MerchantRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    MerchantID = table.Column<int>(nullable: false),
                    TotalOfReciept = table.Column<decimal>(nullable: false),
                    payment = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantReciepts", x => x.MerchantRecieptID);
                    table.ForeignKey(
                        name: "FK_MerchantReciepts_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sarhas",
                columns: table => new
                {
                    SarhaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: false),
                    NumberOfFishermen = table.Column<int>(nullable: false),
                    NumberOfBoxes = table.Column<int>(nullable: false),
                    DateOfSarha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sarhas", x => x.SarhaID);
                    table.ForeignKey(
                        name: "FK_Sarhas_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantRecieptItems",
                columns: table => new
                {
                    MerchantRecieptItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<int>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false),
                    BoatID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    MerchantRecieptID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantRecieptItems", x => x.MerchantRecieptItemID);
                    table.ForeignKey(
                        name: "FK_MerchantRecieptItems_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchantRecieptItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchantRecieptItems_MerchantReciepts_MerchantRecieptID",
                        column: x => x.MerchantRecieptID,
                        principalTable: "MerchantReciepts",
                        principalColumn: "MerchantRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchantRecieptItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoatOwnerReciepts",
                columns: table => new
                {
                    BoatOwnerRecieptID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatID = table.Column<int>(nullable: false),
                    SarhaID = table.Column<int>(nullable: false),
                    TotalBeforePaying = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<decimal>(nullable: false),
                    PaidFromDebts = table.Column<decimal>(nullable: false),
                    TotalAfterPaying = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoatOwnerReciepts", x => x.BoatOwnerRecieptID);
                    table.ForeignKey(
                        name: "FK_BoatOwnerReciepts_Boats_BoatID",
                        column: x => x.BoatID,
                        principalTable: "Boats",
                        principalColumn: "BoatID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoatOwnerReciepts_Sarhas_SarhaID",
                        column: x => x.SarhaID,
                        principalTable: "Sarhas",
                        principalColumn: "SarhaID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Debts_Sarhas",
                columns: table => new
                {
                    SarhaID = table.Column<int>(nullable: false),
                    DebtID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts_Sarhas", x => new { x.DebtID, x.SarhaID });
                    table.ForeignKey(
                        name: "FK_Debts_Sarhas_Debts_DebtID",
                        column: x => x.DebtID,
                        principalTable: "Debts",
                        principalColumn: "DebtID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Debts_Sarhas_Sarhas_SarhaID",
                        column: x => x.SarhaID,
                        principalTable: "Sarhas",
                        principalColumn: "SarhaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fishermen",
                columns: table => new
                {
                    FishermenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishermenName = table.Column<string>(nullable: false),
                    SarhaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishermen", x => x.FishermenID);
                    table.ForeignKey(
                        name: "FK_Fishermen_Sarhas_SarhaID",
                        column: x => x.SarhaID,
                        principalTable: "Sarhas",
                        principalColumn: "SarhaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoatOwnerItems",
                columns: table => new
                {
                    BoatOwnerItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoatOwnerRecieptID = table.Column<int>(nullable: false),
                    FishID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Qty = table.Column<double>(nullable: false),
                    ProductionTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoatOwnerItems", x => x.BoatOwnerItemID);
                    table.ForeignKey(
                        name: "FK_BoatOwnerItems_BoatOwnerReciepts_BoatOwnerRecieptID",
                        column: x => x.BoatOwnerRecieptID,
                        principalTable: "BoatOwnerReciepts",
                        principalColumn: "BoatOwnerRecieptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoatOwnerItems_Fishes_FishID",
                        column: x => x.FishID,
                        principalTable: "Fishes",
                        principalColumn: "FishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoatOwnerItems_ProductionTypes_ProductionTypeID",
                        column: x => x.ProductionTypeID,
                        principalTable: "ProductionTypes",
                        principalColumn: "ProductionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerItems_BoatOwnerRecieptID",
                table: "BoatOwnerItems",
                column: "BoatOwnerRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerItems_FishID",
                table: "BoatOwnerItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerItems_ProductionTypeID",
                table: "BoatOwnerItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerReciepts_BoatID",
                table: "BoatOwnerReciepts",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_BoatOwnerReciepts_SarhaID",
                table: "BoatOwnerReciepts",
                column: "SarhaID");

            migrationBuilder.CreateIndex(
                name: "IX_Boats_TypeID",
                table: "Boats",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Debts_Sarhas_SarhaID",
                table: "Debts_Sarhas",
                column: "SarhaID");

            migrationBuilder.CreateIndex(
                name: "IX_Fishermen_SarhaID",
                table: "Fishermen",
                column: "SarhaID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRecieptItems_BoatID",
                table: "MerchantRecieptItems",
                column: "BoatID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRecieptItems_FishID",
                table: "MerchantRecieptItems",
                column: "FishID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRecieptItems_MerchantRecieptID",
                table: "MerchantRecieptItems",
                column: "MerchantRecieptID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRecieptItems_ProductionTypeID",
                table: "MerchantRecieptItems",
                column: "ProductionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantReciepts_MerchantID",
                table: "MerchantReciepts",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_Sarhas_BoatID",
                table: "Sarhas",
                column: "BoatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoatOwnerItems");

            migrationBuilder.DropTable(
                name: "Cofigs");

            migrationBuilder.DropTable(
                name: "Debts_Sarhas");

            migrationBuilder.DropTable(
                name: "Fishermen");

            migrationBuilder.DropTable(
                name: "MerchantRecieptItems");

            migrationBuilder.DropTable(
                name: "BoatOwnerReciepts");

            migrationBuilder.DropTable(
                name: "Debts");

            migrationBuilder.DropTable(
                name: "Fishes");

            migrationBuilder.DropTable(
                name: "MerchantReciepts");

            migrationBuilder.DropTable(
                name: "ProductionTypes");

            migrationBuilder.DropTable(
                name: "Sarhas");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "Boats");

            migrationBuilder.DropTable(
                name: "BoatTypes");
        }
    }
}
