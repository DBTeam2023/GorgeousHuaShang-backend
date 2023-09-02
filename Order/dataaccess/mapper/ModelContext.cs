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

    public virtual DbSet<EntityFramework.Models.Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=47.97.5.16:1521/xe;USER ID=dbteam;PASSWORD=dbteam;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DBTEAM");

        // 修改 Order 实体类的映射关系
        modelBuilder.Entity<EntityFramework.Models.Order>(entity =>
        {
            entity.ToTable("order");
            entity.HasKey(e => e.ID).HasName("ORDER_PK");
            entity.Property(e => e.ID)
                .HasMaxLength(100)
                .HasColumnName("ORDER_ID")
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(100)
                .HasColumnName("CREATE_TIME")
                .IsUnicode(false);
            entity.Property(e => e.Money)
                .HasColumnName("MONEY");
            entity.Property(e => e.State)
                .HasColumnName("STATE");
            entity.Property(e => e.IsDeleted)
                .HasColumnName("IS_DELETED");
            entity.Property(e => e.PickID)
                .HasColumnName("PICK_ID")
                .HasColumnType("jsonb[]");
            entity.Property(e => e.LogisticID)
                .HasMaxLength(100)
                .HasColumnName("LOGISTICS_ID")
                .IsUnicode(false);
            entity.Property(e => e.UserID)
                .HasMaxLength(100)
                .HasColumnName("USER_ID")
                .IsUnicode(false);
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
