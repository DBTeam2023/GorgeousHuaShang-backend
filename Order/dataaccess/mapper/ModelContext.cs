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

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<EntityFramework.Models.Order> Orders { get; set; }

    public virtual DbSet<OrderPick> OrderPicks { get; set; }

    public virtual DbSet<Pick> Picks { get; set; }

    public virtual DbSet<User> Users { get; set; }

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
                .HasMaxLength(100)
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

        modelBuilder.Entity<EntityFramework.Models.Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("ORDER_PK");

            entity.ToTable("order");

            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.CreateTime)
                .HasPrecision(6)
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.IsDeleted)
                .HasPrecision(1)
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.LogisticsId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("LOGISTICS_ID");
            entity.Property(e => e.Money)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("MONEY");
            entity.Property(e => e.State)
                .HasPrecision(1)
                .HasColumnName("STATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER2USER");
        });

        modelBuilder.Entity<OrderPick>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.PickId }).HasName("ORDER_COMMODITY_PK");

            entity.ToTable("order_pick");

            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.PickId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PICK_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPicks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER");
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
