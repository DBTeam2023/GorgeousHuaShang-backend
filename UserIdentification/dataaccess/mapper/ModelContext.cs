using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserIdentification.dataaccess.DBModels;

namespace UserIdentification.dataaccess.mapper;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("BUYER_PK");

            entity.ToTable("buyer");

            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Age)
                .HasPrecision(3)
                .HasColumnName("AGE");
            entity.Property(e => e.Gender)
                .HasPrecision(1)
                .HasColumnName("GENDER");
            entity.Property(e => e.Height)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("HEIGHT");
            entity.Property(e => e.IsVip)
                .HasPrecision(1)
                .HasColumnName("IS_VIP");
            entity.Property(e => e.ReceiveAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RECEIVE_ADDRESS");
            entity.Property(e => e.Weight)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("WEIGHT");

            entity.HasOne(d => d.User).WithOne(p => p.Buyer)
                .HasForeignKey<Buyer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_BUYER");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SELLER_PK");

            entity.ToTable("seller");

            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SendAddress)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SEND_ADDRESS");

            entity.HasOne(d => d.User).WithOne(p => p.Seller)
                .HasForeignKey<Seller>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_SELLER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("USER_PK");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.LoginTime)
                .HasPrecision(6)
                .HasColumnName("LOGIN_TIME");
            entity.Property(e => e.NickName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NICK_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
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
            entity.HasKey(e => new { e.UserId, e.Balance, e.Status }).HasName("WALLET_PK");

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

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_USER2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
