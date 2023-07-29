using System;
using System.Collections.Generic;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commodity> Commodities { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Coupontemp> Coupontemps { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        modelBuilder.Entity<Commodity>(entity =>
        {
            entity.HasKey(e => e.CommodityId).HasName("COMMODITY_PK");

            entity.ToTable("commodity");

            entity.Property(e => e.CommodityId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_ID");
            entity.Property(e => e.CommodityName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_NAME");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IsDeleted)
                .HasPrecision(1)
                .HasDefaultValueSql("0 ")
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.StockQuantity)
                .HasPrecision(6)
                .HasDefaultValueSql("0 ")
                .HasColumnName("STOCK_QUANTITY");
            entity.Property(e => e.StoreId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");

            entity.HasOne(d => d.Store).WithMany(p => p.Commodities)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_store");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("COUPON_PK");

            entity.ToTable("coupon");

            entity.Property(e => e.CouponId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COUPON_ID");
            entity.Property(e => e.Bar)
                .HasPrecision(10)
                .HasColumnName("BAR");
            entity.Property(e => e.CommodityId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_ID");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Reduction)
                .HasPrecision(10)
                .HasColumnName("REDUCTION");
            entity.Property(e => e.StoreId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TYPE");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Validfrom)
                .HasColumnType("TIMESTAMP(6) WITH LOCAL TIME ZONE")
                .HasColumnName("VALIDFROM");
            entity.Property(e => e.Validto)
                .HasColumnType("TIMESTAMP(6) WITH LOCAL TIME ZONE")
                .HasColumnName("VALIDTO");

            entity.HasOne(d => d.Commodity).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.CommodityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_commodity22");

            entity.HasOne(d => d.Store).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_store22");

            entity.HasOne(d => d.User).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USER22");
        });

        modelBuilder.Entity<Coupontemp>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("COUPONTEMP_PK");

            entity.ToTable("COUPONTEMP");

            entity.Property(e => e.CouponId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COUPON_ID");
            entity.Property(e => e.Bar)
                .HasPrecision(10)
                .HasColumnName("BAR");
            entity.Property(e => e.CommodityId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_ID");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Reduction)
                .HasPrecision(10)
                .HasColumnName("REDUCTION");
            entity.Property(e => e.StoreId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TYPE");
            entity.Property(e => e.Validfrom)
                .HasColumnType("TIMESTAMP(6) WITH LOCAL TIME ZONE")
                .HasColumnName("VALIDFROM");
            entity.Property(e => e.Validto)
                .HasColumnType("TIMESTAMP(6) WITH LOCAL TIME ZONE")
                .HasColumnName("VALIDTO");

            entity.HasOne(d => d.Commodity).WithMany(p => p.Coupontemps)
                .HasForeignKey(d => d.CommodityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_commodity33");

            entity.HasOne(d => d.Store).WithMany(p => p.Coupontemps)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_store33");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("STORE_PK");

            entity.ToTable("store");

            entity.Property(e => e.StoreId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");
            entity.Property(e => e.IsDeleted)
                .HasPrecision(1)
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.Score)
                .HasColumnType("NUMBER(2,1)")
                .HasColumnName("SCORE");
            entity.Property(e => e.StoreName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STORE_NAME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("USER_PK");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.LoginTime)
                .HasPrecision(6)
                .HasColumnName("LOGIN_TIME");
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NICK_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'buyer'   ")
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("WALLET_PK");

            entity.ToTable("wallet");

            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Status)
                .HasPrecision(1)
                .HasColumnName("STATUS");

            entity.HasOne(d => d.User).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.UserId)
                .HasConstraintName("FK_USER2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
