using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FishBusiness.Models;


namespace FishBusiness
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Debts_Sarha>()
                .HasKey(c => new { c.DebtID, c.SarhaID });

            modelBuilder.Entity<Debt_In_Sarha>()
               .HasKey(c => new { c.DebtID, c.SarhaID ,c.PersonID});

            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Debt>()
            //    .HasMany(c => c.Debts_Sarhas)
            //    .WithRequired()
            //    .HasForeignKey(c => c);

            //modelBuilder.Entity<Media>()
            //    .HasMany(c => c.ContractMedias)
            //    .WithRequired()
            //    .HasForeignKey(c => c.MediaId);
        }
    
        public virtual DbSet<Boat> Boats { get; set; }
        public virtual DbSet<BoatOwnerItem> BoatOwnerItems { get; set; }
        public virtual DbSet<BoatOwnerReciept> BoatOwnerReciepts { get; set; }
        public virtual DbSet<BoatType> BoatTypes { get; set; }
        public virtual DbSet<Cofig> Cofigs { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<Debts_Sarha> Debts_Sarhas { get; set; }
        public virtual DbSet<Fish> Fishes { get; set; }
        public virtual DbSet<Fisherman> Fishermen { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<MerchantReciept> MerchantReciepts { get; set; }
        public virtual DbSet<MerchantRecieptItem> MerchantRecieptItems { get; set; }
        public virtual DbSet<ProductionType> ProductionTypes { get; set; }
        public virtual DbSet<Sarha> Sarhas { get; set; }
        public virtual DbSet<SharedBoatsIncome> SharedBoatsIncomes { get; set; }
        
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExternalReceipt> ExternalReceipts { get; set; }
        public virtual DbSet<IncomesOfSharedBoat> IncomesOfSharedBoats { get; set; }
        public DbSet<FishBusiness.Models.IMerchantReciept> IMerchantReciept { get; set; }
        public DbSet<FishBusiness.Models.IMerchantRecieptItem> IMerchantRecieptItem { get; set; }
        public DbSet<FishBusiness.Models.PersonReciept> PersonReciepts { get; set; }
        public DbSet<FishBusiness.Models.PersonRecieptItem> PersonRecieptItems { get; set; }
        public DbSet<FishBusiness.Models.PaidForMerchant> PaidForMerchant { get; set; }
        public DbSet<FishBusiness.Models.Stock> Stocks { get; set; }


        public DbSet<FishBusiness.Models.ISellerReciept> ISellerReciepts { get; set; }
        public DbSet<FishBusiness.Models.ISellerRecieptItem> ISellerRecieptItems { get; set; }
        public DbSet<FishBusiness.Models.TotalOfProfit> TotalOfProfits { get; set; }
        public DbSet<FishBusiness.Models.Person> People { get; set; }
        public DbSet<FishBusiness.Models.Collecting> Collectings { get; set; }
        public DbSet<FishBusiness.Models.AdditionalPayment> AdditionalPayments { get; set; }
        public DbSet<FishBusiness.Models.HalakaHalek> HalakaHaleks { get; set; }
        public DbSet<FishBusiness.Models.LeaderLoan> LeaderLoans { get; set; }
        public DbSet<FishBusiness.Models.LeaderPayback> LeaderPaybacks { get; set; }
        public DbSet<FishBusiness.Models.FathAllahGift> FathAllahGifts { get; set; }
        public DbSet<FishBusiness.Models.Checkout> Checkouts { get; set; }
        public virtual DbSet<Debt_In_Sarha> Debts_In_Sarhas { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<PaidForOperator> PaidForOperators { get; set; }
        public virtual DbSet<OperatorDeal> OperatorDeals { get; set; }


        public virtual DbSet<BranchOffice> BranchOffices { get; set; }
        public virtual DbSet<StockRec> StockRecs { get; set; }
        public virtual DbSet<StockRecItem> StockRecItems { get; set; }

        public virtual DbSet<SellerRec> SellerRecs { get; set; }
        public virtual DbSet<SellerRecItem> SellerRecItems { get; set; }
        public virtual DbSet<PaidForSeller> PaidForSellers { get; set; }
        public virtual DbSet<PaidForBoat> PaidForBoats { get; set; }
        public virtual DbSet<AdditionalForOffice> AdditionalForOffices { get; set; }









    }
}
