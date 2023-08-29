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

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SellerStore> SellerStores { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

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

            entity.Ignore(e => e.SellerStores);
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
            entity.Ignore(e => e.SellerStores);
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
