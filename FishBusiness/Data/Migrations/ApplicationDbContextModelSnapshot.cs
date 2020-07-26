﻿// <auto-generated />
using System;
using FishBusiness;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FishBusiness.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FishBusiness.Models.Boat", b =>
                {
                    b.Property<int>("BoatID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BoatImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoatLeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoatLicenseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoatName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DebtsOfHalek")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DebtsOfStartingWork")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("IncomeOfSharedBoat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.HasKey("BoatID");

                    b.HasIndex("TypeID");

                    b.ToTable("Boats");
                });

            modelBuilder.Entity("FishBusiness.Models.BoatOwnerItem", b =>
                {
                    b.Property<int>("BoatOwnerItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatOwnerRecieptID")
                        .HasColumnType("int");

                    b.Property<int>("FishID")
                        .HasColumnType("int");

                    b.Property<int>("ProductionTypeID")
                        .HasColumnType("int");

                    b.Property<double>("Qty")
                        .HasColumnType("float");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BoatOwnerItemID");

                    b.HasIndex("BoatOwnerRecieptID");

                    b.HasIndex("FishID");

                    b.HasIndex("ProductionTypeID");

                    b.ToTable("BoatOwnerItems");
                });

            modelBuilder.Entity("FishBusiness.Models.BoatOwnerReciept", b =>
                {
                    b.Property<int>("BoatOwnerRecieptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatID")
                        .HasColumnType("int");

                    b.Property<decimal>("Commission")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("FinalIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PaidFromDebts")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SarhaID")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAfterPaying")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalBeforePaying")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BoatOwnerRecieptID");

                    b.HasIndex("BoatID");

                    b.HasIndex("SarhaID");

                    b.ToTable("BoatOwnerReciepts");
                });

            modelBuilder.Entity("FishBusiness.Models.BoatType", b =>
                {
                    b.Property<int>("TypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeID");

                    b.ToTable("BoatTypes");
                });

            modelBuilder.Entity("FishBusiness.Models.Cofig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Cofigs");
                });

            modelBuilder.Entity("FishBusiness.Models.Debt", b =>
                {
                    b.Property<int>("DebtID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DebtName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DebtID");

                    b.ToTable("Debts");
                });

            modelBuilder.Entity("FishBusiness.Models.Debts_Sarha", b =>
                {
                    b.Property<int>("DebtID")
                        .HasColumnType("int");

                    b.Property<int>("SarhaID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DebtID", "SarhaID");

                    b.HasIndex("SarhaID");

                    b.ToTable("Debts_Sarhas");
                });

            modelBuilder.Entity("FishBusiness.Models.Expense", b =>
                {
                    b.Property<int>("ExpenseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatID")
                        .HasColumnType("int");

                    b.Property<string>("Cause")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ExpenseID");

                    b.HasIndex("BoatID");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("FishBusiness.Models.Fish", b =>
                {
                    b.Property<int>("FishID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FishName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FishID");

                    b.ToTable("Fishes");
                });

            modelBuilder.Entity("FishBusiness.Models.Fisherman", b =>
                {
                    b.Property<int>("FishermenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FishermenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SarhaID")
                        .HasColumnType("int");

                    b.HasKey("FishermenID");

                    b.HasIndex("SarhaID");

                    b.ToTable("Fishermen");
                });

            modelBuilder.Entity("FishBusiness.Models.Merchant", b =>
                {
                    b.Property<int>("MerchantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PreviousDebts")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MerchantID");

                    b.ToTable("Merchants");
                });

            modelBuilder.Entity("FishBusiness.Models.MerchantReciept", b =>
                {
                    b.Property<int>("MerchantRecieptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("MerchantID")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalOfReciept")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("payment")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MerchantRecieptID");

                    b.HasIndex("MerchantID");

                    b.ToTable("MerchantReciepts");
                });

            modelBuilder.Entity("FishBusiness.Models.MerchantRecieptItem", b =>
                {
                    b.Property<int>("MerchantRecieptItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatID")
                        .HasColumnType("int");

                    b.Property<int>("FishID")
                        .HasColumnType("int");

                    b.Property<int>("MerchantRecieptID")
                        .HasColumnType("int");

                    b.Property<int>("ProductionTypeID")
                        .HasColumnType("int");

                    b.Property<int>("Qty")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MerchantRecieptItemID");

                    b.HasIndex("BoatID");

                    b.HasIndex("FishID");

                    b.HasIndex("MerchantRecieptID");

                    b.HasIndex("ProductionTypeID");

                    b.ToTable("MerchantRecieptItems");
                });

            modelBuilder.Entity("FishBusiness.Models.ProductionType", b =>
                {
                    b.Property<int>("ProductionTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProductionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductionTypeID");

                    b.ToTable("ProductionTypes");
                });

            modelBuilder.Entity("FishBusiness.Models.Sarha", b =>
                {
                    b.Property<int>("SarhaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfSarha")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfBoxes")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfFishermen")
                        .HasColumnType("int");

                    b.HasKey("SarhaID");

                    b.HasIndex("BoatID");

                    b.ToTable("Sarhas");
                });

            modelBuilder.Entity("FishBusiness.Models.SharedBoatsIncome", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoatID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Income")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID");

                    b.HasIndex("BoatID");

                    b.ToTable("SharedBoatsIncomes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FishBusiness.Models.Boat", b =>
                {
                    b.HasOne("FishBusiness.Models.BoatType", "BoatType")
                        .WithMany("Boats")
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.BoatOwnerItem", b =>
                {
                    b.HasOne("FishBusiness.Models.BoatOwnerReciept", "BoatOwnerReciept")
                        .WithMany("BoatOwnerItems")
                        .HasForeignKey("BoatOwnerRecieptID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.Fish", "Fish")
                        .WithMany("BoatOwnerItems")
                        .HasForeignKey("FishID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.ProductionType", "ProductionType")
                        .WithMany("BoatOwnerItems")
                        .HasForeignKey("ProductionTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.BoatOwnerReciept", b =>
                {
                    b.HasOne("FishBusiness.Models.Boat", "Boat")
                        .WithMany("BoatOwnerReciepts")
                        .HasForeignKey("BoatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.Sarha", "Sarha")
                        .WithMany("BoatOwnerReciepts")
                        .HasForeignKey("SarhaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.Debts_Sarha", b =>
                {
                    b.HasOne("FishBusiness.Models.Debt", "Debt")
                        .WithMany("Debts_Sarhas")
                        .HasForeignKey("DebtID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.Sarha", "Sarha")
                        .WithMany("Debts_Sarhas")
                        .HasForeignKey("SarhaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.Expense", b =>
                {
                    b.HasOne("FishBusiness.Models.Boat", "Boat")
                        .WithMany("Expenses")
                        .HasForeignKey("BoatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.Fisherman", b =>
                {
                    b.HasOne("FishBusiness.Models.Sarha", "Sarha")
                        .WithMany("Fishermen")
                        .HasForeignKey("SarhaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.MerchantReciept", b =>
                {
                    b.HasOne("FishBusiness.Models.Merchant", "Merchant")
                        .WithMany("MerchantReciepts")
                        .HasForeignKey("MerchantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.MerchantRecieptItem", b =>
                {
                    b.HasOne("FishBusiness.Models.Boat", "Boat")
                        .WithMany("MerchantRecieptItems")
                        .HasForeignKey("BoatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.Fish", "Fish")
                        .WithMany("MerchantRecieptItems")
                        .HasForeignKey("FishID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.MerchantReciept", "MerchantReciept")
                        .WithMany("MerchantRecieptItems")
                        .HasForeignKey("MerchantRecieptID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FishBusiness.Models.ProductionType", "ProductionType")
                        .WithMany("MerchantRecieptItems")
                        .HasForeignKey("ProductionTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.Sarha", b =>
                {
                    b.HasOne("FishBusiness.Models.Boat", "Boat")
                        .WithMany("Sarhas")
                        .HasForeignKey("BoatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FishBusiness.Models.SharedBoatsIncome", b =>
                {
                    b.HasOne("FishBusiness.Models.Boat", "Boat")
                        .WithMany("SharedBoatsIncomes")
                        .HasForeignKey("BoatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
