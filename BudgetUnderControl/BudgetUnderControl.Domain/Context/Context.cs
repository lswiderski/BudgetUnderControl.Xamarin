﻿
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Collections.Generic;
using BudgetUnderControl.Common;
using System;

namespace BudgetUnderControl.Domain
{
    public class Context : DbContext, IDisposable
    {
        private string databasePath { get; set; }


        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountGroup> AccountGroup { get; set; }
        public virtual DbSet<AccountSnapshot> AccountSnapshot { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Icon> Icons { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagToTransaction> TagsToTransactions { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }

        public static Context Create(IContextConfig contextConfig)
        {
            var context = new Context(contextConfig);
            
            return context;
        }

        protected Context()
        {
            Database.Migrate();
        }

        protected Context(string dbPath)
        {
            this.databasePath = dbPath;
            Database.Migrate();
        }

        protected Context(IContextConfig config)
        {
            var dbPath = config.DbPath;
            this.databasePath = dbPath;
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<AccountGroup>().ToTable("AccountGroup");
            modelBuilder.Entity<AccountSnapshot>().ToTable("AccountSnapshot");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRate");
            modelBuilder.Entity<File>().ToTable("File");
            modelBuilder.Entity<Icon>().ToTable("Icon");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<TagToTransaction>().ToTable("TagToTransaction");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Transfer>().ToTable("Transfer");



            modelBuilder.Entity<Account>()
                .HasOne(x => x.AccountGroup)
                .WithMany(y => y.Accounts)
                .HasForeignKey(x => x.AccountGroupId)
                .HasConstraintName("ForeignKey_Account_AccountGroup");

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
                .HasConstraintName("ForeignKey_AccountSnapshot_LastTransaction");

            modelBuilder.Entity<AccountSnapshot>()
                .HasOne(x => x.LastTransaction)
                .WithMany(y => y.AccountSnapshots)
                .HasForeignKey(x => x.LastTransactionId)
                .HasConstraintName("ForeignKey_AccountSnapshot_LastTransaction");

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(x => x.FromCurrency)
                .WithMany(y => y.FromExchangeRates)
                .HasForeignKey(x => x.FromCurrencyId)
                .HasConstraintName("ForeignKey_ExchangeRate_FromCurrency");

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(x => x.ToCurrency)
                .WithMany(y => y.ToExchangeRates)
                .HasForeignKey(x => x.ToCurrencyId)
                .HasConstraintName("ForeignKey_ExchangeRate_ToCurrency");

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
                .HasConstraintName("ForeignKey_Transfer_FromTransaction");

            modelBuilder.Entity<Transfer>()
                .HasOne(x => x.ToTransaction)
                .WithMany(y => y.ToTransfers)
                .HasForeignKey(x => x.ToTransactionId)
                .HasConstraintName("ForeignKey_Transfer_ToTransaction");

            modelBuilder.Entity<AccountSnapshot>()
                .HasOne(x => x.PreviousAccountSnapshot)
                .WithMany()
                .HasForeignKey(e => e.PreviousAccountSnapshotId);
        }

        }
    }