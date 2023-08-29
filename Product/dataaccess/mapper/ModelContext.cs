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

    public virtual DbSet<Order> Orders { get; set; }

    //public virtual DbSet<Category> Categories { get; set; }

    //public virtual DbSet<CommodityGeneral> CommodityGenerals { get; set; }

    //public virtual DbSet<CommodityProperty> CommodityProperties { get; set; }

    //public virtual DbSet<Pick> Picks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("ORDER_PK");
            entity.ToTable("order");
            entity.Property(e => e.ID)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.Time)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.Money)
             .HasMaxLength(100)
             .IsUnicode(false)
             .HasColumnName("MONEY");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.IsDeleted)
             .HasMaxLength(100)
             .IsUnicode(false)
             .HasColumnName("IS_DELETED");
            entity.Property(e => e.LogisticsID)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("LOGISTICS_ID");
            entity.Property(e => e.StoreID)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("STORE_ID");
            entity.Property(e => e.UserID)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("USER_ID");
        });

        //modelBuilder.Entity<Category>(entity =>
        //{
        //    entity.HasKey(e => e.CommodityId).HasName("CATEGORY_PK");

        //    entity.ToTable("CATEGORY");

        //    entity.Property(e => e.CommodityId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("COMMODITY_ID");
        //    entity.Property(e => e.Type)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("TYPE");

        //    entity.HasOne(d => d.Commodity).WithOne(p => p.Category)
        //        .HasForeignKey<Category>(d => d.CommodityId)
        //        .HasConstraintName("FK_COMMODITY_GENERAL4");
        //});

        //modelBuilder.Entity<CommodityGeneral>(entity =>
        //{
        //    entity.HasKey(e => e.CommodityId).HasName("COMMODITY_GENERAL_PK");

        //    entity.ToTable("COMMODITY_GENERAL");

        //    entity.Property(e => e.CommodityId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("COMMODITY_ID");
        //    entity.Property(e => e.CommodityName)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("COMMODITY_NAME");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("DESCRIPTION");
        //    entity.Property(e => e.IsDeleted)
        //        .IsRequired()
        //        .HasPrecision(1)
        //        .HasDefaultValueSql("0 ")
        //        .HasColumnName("IS_DELETED");
        //    entity.Property(e => e.Price)
        //        .HasColumnType("NUMBER(7,2)")
        //        .HasColumnName("PRICE");
        //    entity.Property(e => e.StoreId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("STORE_ID");
        //});

        //modelBuilder.Entity<CommodityProperty>(entity =>
        //{
        //    entity.HasKey(e => new { e.CommodityId, e.PropertyType, e.PropertyValue }).HasName("PROPERTY_PK");

        //    entity.ToTable("COMMODITY_PROPERTY");

        //    entity.Property(e => e.CommodityId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("COMMODITY_ID");
        //    entity.Property(e => e.PropertyType)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("PROPERTY_TYPE");
        //    entity.Property(e => e.PropertyValue)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("PROPERTY_VALUE");

        //    entity.HasOne(d => d.Commodity).WithMany(p => p.CommodityProperties)
        //        .HasForeignKey(d => d.CommodityId)
        //        .HasConstraintName("FK_COMMODITY_GENERAL3");
        //});

        //modelBuilder.Entity<Pick>(entity =>
        //{
        //    entity.HasKey(e => new { e.PickId, e.PropertyType }).HasName("PICK_PK");

        //    entity.ToTable("PICK");

        //    entity.Property(e => e.PickId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("PICK_ID");
        //    entity.Property(e => e.PropertyType)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("PROPERTY_TYPE");
        //    entity.Property(e => e.CommodityId)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("COMMODITY_ID");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("DESCRIPTION");
        //    entity.Property(e => e.IsDeleted)
        //        .IsRequired()
        //        .HasPrecision(1)
        //        .HasDefaultValueSql("0 ")
        //        .HasColumnName("IS_DELETED");
        //    entity.Property(e => e.Price)
        //        .HasColumnType("NUMBER(7,2)")
        //        .HasColumnName("PRICE");
        //    entity.Property(e => e.PropertyValue)
        //        .HasMaxLength(100)
        //        .IsUnicode(false)
        //        .HasColumnName("PROPERTY_VALUE");

        //    entity.HasOne(d => d.CommodityProperty).WithMany(p => p.Picks)
        //        .HasForeignKey(d => new { d.CommodityId, d.PropertyType, d.PropertyValue })
        //        .OnDelete(DeleteBehavior.Cascade)
        //        .HasConstraintName("FK_COMMODITY_PROPERTY");
        //});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
