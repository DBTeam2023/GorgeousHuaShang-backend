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

    public virtual DbSet<CommodityGeneral> CommodityGenerals { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Coupontemp> Coupontemps { get; set; }

    public virtual DbSet<Pick> Picks { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SellerStore> SellerStores { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        modelBuilder.Entity<CommodityGeneral>(entity =>
        {
            entity.HasKey(e => e.CommodityId).HasName("COMMODITY_GENERAL_PK");

            entity.ToTable("COMMODITY_GENERAL");

            entity.Property(e => e.CommodityId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_ID");
            entity.Property(e => e.CommodityName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_NAME");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IsDeleted)
                .HasPrecision(1)
                .HasDefaultValueSql("0  ")
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.StoreId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");

            entity.HasOne(d => d.Store).WithMany(p => p.CommodityGenerals)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_STORE1");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => new { e.CouponId, e.UserId }).HasName("COUPON_PK");

            entity.ToTable("coupon");

            entity.Property(e => e.CouponId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COUPON_ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Bar)
                .HasPrecision(10)
                .HasColumnName("BAR");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Reduction)
                .HasPrecision(10)
                .HasColumnName("REDUCTION");
            entity.Property(e => e.StoreId)
                .HasMaxLength(100)
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

            entity.HasOne(d => d.Store).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_store22");

            entity.HasOne(d => d.User).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.UserId)
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
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Reduction)
                .HasPrecision(10)
                .HasColumnName("REDUCTION");
            entity.Property(e => e.StoreId)
                .HasMaxLength(100)
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

            entity.HasOne(d => d.Store).WithMany(p => p.Coupontemps)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_store33");
        });

        modelBuilder.Entity<Pick>(entity =>
        {
            entity.HasKey(e => new { e.PickId, e.PropertyType }).HasName("PICK_PK");

            entity.ToTable("PICK");

            entity.Property(e => e.PickId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PICK_ID");
            entity.Property(e => e.PropertyType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PROPERTY_TYPE");
            entity.Property(e => e.CommodityId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COMMODITY_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IsDeleted)
                .HasPrecision(1)
                .HasDefaultValueSql("0  ")
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.PropertyValue)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PROPERTY_VALUE");
            entity.Property(e => e.Stock)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("STOCK");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SELLER_PK");

            entity.ToTable("seller");

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SendAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SEND_ADDRESS");

            entity.HasOne(d => d.User).WithOne(p => p.Seller)
                .HasForeignKey<Seller>(d => d.UserId)
                .HasConstraintName("FK_user22");
        });

        modelBuilder.Entity<SellerStore>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.UserId }).HasName("SELLER_STORE_PK");

            entity.ToTable("SELLER_STORE");

            entity.Property(e => e.StoreId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Ismanager)
                .HasDefaultValueSql("0  ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISMANAGER");

            entity.HasOne(d => d.Store).WithMany(p => p.SellerStores)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_store2");

            entity.HasOne(d => d.User).WithMany(p => p.SellerStores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_seller");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("STORE_PK");

            entity.ToTable("store");

            entity.Property(e => e.StoreId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STORE_ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
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
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'buyer'   ")
                .HasColumnName("TYPE");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("WALLET_PK");

            entity.ToTable("wallet");

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
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
