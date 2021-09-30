using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Collections.Generic;
using BudgetUnderControl.Common;
using System;
using BudgetUnderControl.Common.Enums;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace BudgetUnderControl.MobileDomain
{
    public class Context : DbContext, IDisposable
    {
        private readonly IContextConfig config;
        
        private string databasePath { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountSnapshot> AccountSnapshot { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<FileToTransaction> FilesToTransactions { get; set; }
        public virtual DbSet<Icon> Icons { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagToTransaction> TagsToTransactions { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Synchronization> Synchronizations { get; set; }
        

        public static Context Create(IContextConfig contextConfig)
        {
            var context = new Context(contextConfig);
             //context.Database.EnsureDeleted();
             //context.Database.EnsureCreated();
            return context;
        }

        public static Context CreateTest(DbContextOptions options, IContextConfig config)
        {
            var context = new Context(options, config);
            
            context.Database.EnsureCreated();
            return context;
        }

        protected Context()
        {
            
        }

        public Context(DbContextOptions options, IContextConfig config) : base(options)
        {
            this.config = config;
            if (config.Application == ApplicationType.Test)
            {
                Database.Migrate();
            }
        }

        public Context(IContextConfig config)
        {
            this.config = config;
            if (config.Application == ApplicationType.Mobile || config.Application == ApplicationType.Web)
            {
                Database.Migrate();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(config!= null)
            {
                if (config.Application == ApplicationType.Mobile || config.Application == ApplicationType.SQLiteMigrations)
                {
                    optionsBuilder.UseSqlite(config.ConnectionString, options => options.MigrationsAssembly("BudgetUnderControl.Migrations.SQLite"));
                }
                else if(config.Application == ApplicationType.Test)
                {
                    
                }
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<AccountSnapshot>().ToTable("AccountSnapshot");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRate");
            modelBuilder.Entity<File>().ToTable("File");
            modelBuilder.Entity<FileToTransaction>().ToTable("FileToTransaction");
            modelBuilder.Entity<Icon>().ToTable("Icon");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<TagToTransaction>().ToTable("TagToTransaction");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Transfer>().ToTable("Transfer");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Synchronization>().ToTable("Synchronization");

            modelBuilder.Entity<Account>()
                .HasOne(x => x.Currency)
                .WithMany(y => y.Accounts)
                .HasForeignKey(x => x.CurrencyId)
                .HasConstraintName("ForeignKey_Account_Currency");

            modelBuilder.Entity<AccountSnapshot>()
                .HasOne(x => x.Account)
                .WithMany(y => y.AccountSnapshots)
                .HasForeignKey(x => x.AccountId)
                .HasConstraintName("ForeignKey_AccountSnapshot_Account");


            modelBuilder.Entity<AccountSnapshot>()
                .HasOne(x => x.LastTransaction)
                .WithMany(y => y.AccountSnapshots)
                .HasForeignKey(x => x.LastTransactionId)
                .HasConstraintName("ForeignKey_AccountSnapshot_LastTransaction")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(x => x.FromCurrency)
                .WithMany(y => y.FromExchangeRates)
                .HasForeignKey(x => x.FromCurrencyId)
                .HasConstraintName("ForeignKey_ExchangeRate_FromCurrency")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(x => x.ToCurrency)
                .WithMany(y => y.ToExchangeRates)
                .HasForeignKey(x => x.ToCurrencyId)
                .HasConstraintName("ForeignKey_ExchangeRate_ToCurrency")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TagToTransaction>()
               .HasOne(x => x.Tag)
               .WithMany(y => y.TagToTransactions)
               .HasForeignKey(x => x.TagId)
               .HasConstraintName("ForeignKey_TagToTransaction_Tag");

            modelBuilder.Entity<TagToTransaction>()
               .HasOne(x => x.Transaction)
               .WithMany(y => y.TagsToTransaction)
               .HasForeignKey(x => x.TransactionId)
               .HasConstraintName("ForeignKey_TagToTransaction_Transaction");

            modelBuilder.Entity<Transaction>()
               .HasOne(x => x.Account)
               .WithMany(y => y.Transactions)
               .HasForeignKey(x => x.AccountId)
               .HasConstraintName("ForeignKey_Transaction_Account");

            modelBuilder.Entity<Transaction>()
              .HasOne(x => x.Category)
              .WithMany(y => y.Transactions)
              .HasForeignKey(x => x.CategoryId)
              .HasConstraintName("ForeignKey_Transaction_Category");

            modelBuilder.Entity<Transfer>()
                .HasOne(x => x.FromTransaction)
                .WithMany(y => y.FromTransfers)
                .HasForeignKey(x => x.FromTransactionId)
                .HasConstraintName("ForeignKey_Transfer_FromTransaction")
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(x => x.ToTransaction)
                .WithMany(y => y.ToTransfers)
                .HasForeignKey(x => x.ToTransactionId)
                .HasConstraintName("ForeignKey_Transfer_ToTransaction")
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSnapshot>()
                .HasOne(x => x.PreviousAccountSnapshot)
                .WithMany()
                .HasForeignKey(e => e.PreviousAccountSnapshotId);

            modelBuilder.Entity<Account>()
                 .HasOne(x => x.Owner)
                .WithMany(y => y.Accounts)
                .HasForeignKey(x => x.OwnerId)
                .HasConstraintName("ForeignKey_Account_User")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                 .HasOne(x => x.AddedBy)
                .WithMany(y => y.Transactions)
                .HasForeignKey(x => x.AddedById)
                .HasConstraintName("ForeignKey_Transaction_User")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileToTransaction>()
              .HasOne(x => x.File)
              .WithMany(y => y.FileToTransactions)
              .HasForeignKey(x => x.FileId)
              .HasConstraintName("ForeignKey_FileToTransaction_File");

            modelBuilder.Entity<FileToTransaction>()
               .HasOne(x => x.Transaction)
               .WithMany(y => y.FilesToTransaction)
               .HasForeignKey(x => x.TransactionId)
               .HasConstraintName("ForeignKey_FileToTransaction_Transaction");

            modelBuilder.Entity<Synchronization>()
               .HasOne(x => x.User)
              .WithMany(y => y.Synchronizations)
              .HasForeignKey(x => x.UserId)
              .HasConstraintName("ForeignKey_Synchronization_User")
              .OnDelete(DeleteBehavior.Restrict);
        }

        }
    }
