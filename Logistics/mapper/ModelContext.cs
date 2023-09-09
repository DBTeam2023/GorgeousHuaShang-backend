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

    public virtual DbSet<Logistic> Logistics { get; set; }

    public virtual DbSet<Logisticsinfo> Logisticsinfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        modelBuilder.Entity<Logistic>(entity =>
        {
            entity.HasKey(e => e.LogisticsId).HasName("LOGISTICS_PK");

            entity.ToTable("logistics");

            entity.Property(e => e.LogisticsId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("LOGISTICS_ID");
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COMPANY");
            entity.Property(e => e.EndTime)
                .HasPrecision(6)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("END_TIME");
            entity.Property(e => e.PickAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PICK_ADDRESS");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SHIP_ADDRESS");
            entity.Property(e => e.StartTime)
                .HasPrecision(6)
                .HasColumnName("START_TIME");
        });

        modelBuilder.Entity<Logisticsinfo>(entity =>
        {
            entity.HasKey(e => new { e.LogisticsId, e.ArrivePlace, e.ArriveTime }).HasName("NEWTABLE_PK");

            entity.ToTable("LOGISTICSINFO");

            entity.Property(e => e.LogisticsId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("LOGISTICS_ID");
            entity.Property(e => e.ArrivePlace)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ARRIVE_PLACE");
            entity.Property(e => e.ArriveTime)
                .HasPrecision(6)
                .HasColumnName("ARRIVE_TIME");

            entity.HasOne(d => d.Logistics).WithMany(p => p.Logisticsinfos)
                .HasForeignKey(d => d.LogisticsId)
                .HasConstraintName("FK_logistics");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
