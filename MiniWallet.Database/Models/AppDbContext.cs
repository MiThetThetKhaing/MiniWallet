using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniWallet.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblDepositWithdraw> TblDepositWithdraws { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblWallet> TblWallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-6QTP69L\\MSSQLSERVER2022;Database=DotNetTrainingBatch5;User Id=sa;Password=sa123;TrustServerCertificate=True;");

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
