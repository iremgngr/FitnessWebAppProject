using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace loginDemo.Models;

public partial class UserToDoDatabaseContext : DbContext
{
    public UserToDoDatabaseContext()
    {
    }

    public UserToDoDatabaseContext(DbContextOptions<UserToDoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblTodo> TblTodos { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserRate> UserRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = WebApplication.CreateBuilder();
        var connectionString = builder.Configuration.GetConnectionString ("MyConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.ToTable("tbl_city");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
        });

        modelBuilder.Entity<TblTodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_todo__3213E83F716950B9");

            entity.ToTable("tbl_todo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("category");
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("difficultyLevel"); // New property mapping
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("title");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("userId");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.ToTable("userDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("userId");
            entity.Property(e => e.Biography) // Yeni eklenen özellik için yapılandırma
                .HasColumnName("biography")
                .HasMaxLength(100);
        });

        modelBuilder.Entity<UserRate>(entity =>
        {
            entity.ToTable("userRate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.TodoId).HasColumnName("todoId");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("userId");
            entity.Property(e => e.IsSaved).HasColumnName("IsSaved"); // Yeni alan için konfigürasyon
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
