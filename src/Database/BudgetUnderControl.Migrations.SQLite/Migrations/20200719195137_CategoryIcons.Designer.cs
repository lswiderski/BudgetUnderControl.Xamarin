﻿// <auto-generated />
using System;
using BudgetUnderControl.MobileDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migrations.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200719195137_CategoryIcons")]
    partial class CategoryIcons
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6");

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconBackgroundColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconFont")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconGlyph")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsIncludedToTotal")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(250);

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParentAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountGroupId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.AccountGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("AccountGroup");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.AccountSnapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("LastTransactionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PreviousAccountSnapshotId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LastTransactionId");

                    b.HasIndex("PreviousAccountSnapshotId");

                    b.ToTable("AccountSnapshot");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconBackgroundColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconFont")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconGlyph")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT")
                        .HasMaxLength(3);

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(250);

                    b.Property<short>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FromCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<double>("Rate")
                        .HasColumnType("REAL");

                    b.Property<int>("ToCurrencyId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FromCurrencyId");

                    b.HasIndex("ToCurrencyId");

                    b.ToTable("ExchangeRate");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MimeType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.FileToTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FileId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("TransactionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("TransactionId");

                    b.ToTable("FileToTransaction");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Icon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Icon");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Synchronization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Component")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComponentId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastSyncAt")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Synchronization");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.TagToTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TagId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransactionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TagId");

                    b.HasIndex("TransactionId");

                    b.ToTable("TagToTransaction");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AddedById")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double?>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AddedById");

                    b.HasIndex("CategoryId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FromTransactionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ToTransactionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FromTransactionId");

                    b.HasIndex("ToTransactionId");

                    b.ToTable("Transfer");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(150);

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Account", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.AccountGroup", "AccountGroup")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountGroupId")
                        .HasConstraintName("ForeignKey_Account_AccountGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("ForeignKey_Account_Currency")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.User", "Owner")
                        .WithMany("Accounts")
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("ForeignKey_Account_User")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.AccountGroup", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.User", "Owner")
                        .WithMany("AccountGroups")
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("ForeignKey_AccountGroup_User")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.AccountSnapshot", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.Account", "Account")
                        .WithMany("AccountSnapshots")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("ForeignKey_AccountSnapshot_Account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Transaction", "LastTransaction")
                        .WithMany("AccountSnapshots")
                        .HasForeignKey("LastTransactionId")
                        .HasConstraintName("ForeignKey_AccountSnapshot_LastTransaction")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.AccountSnapshot", "PreviousAccountSnapshot")
                        .WithMany()
                        .HasForeignKey("PreviousAccountSnapshotId");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Category", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.ExchangeRate", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.Currency", "FromCurrency")
                        .WithMany("FromExchangeRates")
                        .HasForeignKey("FromCurrencyId")
                        .HasConstraintName("ForeignKey_ExchangeRate_FromCurrency")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Currency", "ToCurrency")
                        .WithMany("ToExchangeRates")
                        .HasForeignKey("ToCurrencyId")
                        .HasConstraintName("ForeignKey_ExchangeRate_ToCurrency")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.FileToTransaction", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.File", "File")
                        .WithMany("FileToTransactions")
                        .HasForeignKey("FileId")
                        .HasConstraintName("ForeignKey_FileToTransaction_File")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Transaction", "Transaction")
                        .WithMany("FilesToTransaction")
                        .HasForeignKey("TransactionId")
                        .HasConstraintName("ForeignKey_FileToTransaction_Transaction")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Synchronization", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Tag", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.TagToTransaction", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.Tag", "Tag")
                        .WithMany("TagToTransactions")
                        .HasForeignKey("TagId")
                        .HasConstraintName("ForeignKey_TagToTransaction_Tag")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Transaction", "Transaction")
                        .WithMany("TagsToTransaction")
                        .HasForeignKey("TransactionId")
                        .HasConstraintName("ForeignKey_TagToTransaction_Transaction")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Transaction", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("ForeignKey_Transaction_Account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.User", "AddedBy")
                        .WithMany("Transactions")
                        .HasForeignKey("AddedById")
                        .HasConstraintName("ForeignKey_Transaction_User")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ForeignKey_Transaction_Category");
                });

            modelBuilder.Entity("BudgetUnderControl.MobileDomain.Transfer", b =>
                {
                    b.HasOne("BudgetUnderControl.MobileDomain.Transaction", "FromTransaction")
                        .WithMany("FromTransfers")
                        .HasForeignKey("FromTransactionId")
                        .HasConstraintName("ForeignKey_Transfer_FromTransaction")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BudgetUnderControl.MobileDomain.Transaction", "ToTransaction")
                        .WithMany("ToTransfers")
                        .HasForeignKey("ToTransactionId")
                        .HasConstraintName("ForeignKey_Transfer_ToTransaction")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}