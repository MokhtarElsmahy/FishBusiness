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


    }
}
