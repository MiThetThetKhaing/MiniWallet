using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniWallet.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<TblDepositWithdraw> TblDepositWithdraws { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblWallet> TblWallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblDepositWithdraw>(entity =>
        {
            entity.ToTable("Tbl_Deposit_Withdraw");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.No).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("Tbl_Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(225);
            entity.Property(e => e.ReceiverMobileNo).HasMaxLength(50);
            entity.Property(e => e.SenderMobileNo).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionNo).HasMaxLength(50);
        });

        modelBuilder.Entity<TblWallet>(entity =>
        {
            entity.HasKey(e => e.WalletUserId);

            entity.ToTable("Tbl_Wallet");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FullName).HasMaxLength(225);
            entity.Property(e => e.MobileNo).HasMaxLength(225);
            entity.Property(e => e.WalletUserName).HasMaxLength(225);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
