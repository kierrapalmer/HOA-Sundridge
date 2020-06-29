using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Models {

    public class HOAContext : DbContext {

        public HOAContext(DbContextOptions<HOAContext> options) : base(options) {
        }

        public DbSet<Owner> Owner { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<ClassifiedCategory> ClassifiedCategory { get; set; }
        public DbSet<OwnerHistory> OwnerHistory { get; set; }
        public DbSet<LotInventory> LotInventory { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<HistoryType> HistoryType { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<ClassifiedListing> ClassifiedListing { get; set; }
        public DbSet<KeyHistory> KeyHistory { get; set; }
        public DbSet<Key> Key { get; set; }
        public DbSet<OwnerContactType> OwnerContactType { get; set; }
        public DbSet<ContactType> ContactType { get; set; }
        public DbSet<CommonAreaAsset> CommonAreaAsset { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Owner>().ToTable("HOA_Owner");
            modelBuilder.Entity<Lot>().ToTable("HOA_Lot");
            modelBuilder.Entity<Address>().ToTable("HOA_Address");
            modelBuilder.Entity<OwnerHistory>().ToTable("HOA_OwnerHistory");
            modelBuilder.Entity<LotInventory>().ToTable("HOA_LotInventory");
            modelBuilder.Entity<Inventory>().ToTable("HOA_Inventory");
            modelBuilder.Entity<Transaction>().ToTable("HOA_Transaction");
            modelBuilder.Entity<HistoryType>().ToTable("HOA_HistoryType");
            modelBuilder.Entity<TransactionType>().ToTable("HOA_TransactionType");
            modelBuilder.Entity<File>().ToTable("HOA_File");
            modelBuilder.Entity<ClassifiedListing>().ToTable("HOA_ClassifiedListing");
            modelBuilder.Entity<ClassifiedCategory>().ToTable("HOA_ClassifiedCategory");
            modelBuilder.Entity<KeyHistory>().ToTable("HOA_KeyHistory");
            modelBuilder.Entity<Key>().ToTable("HOA_Key");
            modelBuilder.Entity<OwnerContactType>().ToTable("HOA_OwnerContactType");
            modelBuilder.Entity<ContactType>().ToTable("HOA_ContactType");
            modelBuilder.Entity<CommonAreaAsset>().ToTable("HOA_CommonAreaAsset");
            modelBuilder.Entity<Maintenance>().ToTable("HOA_Maintenance");
            modelBuilder.Entity<User>().ToTable("HOA_User");

            modelBuilder.Entity<OwnerHistory>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(s => s.OwnerID);

            modelBuilder.Entity<OwnerHistory>()
                .HasOne(p => p.Lot)
                .WithMany()
                .HasForeignKey(s => s.LotID);
        }
    }
}