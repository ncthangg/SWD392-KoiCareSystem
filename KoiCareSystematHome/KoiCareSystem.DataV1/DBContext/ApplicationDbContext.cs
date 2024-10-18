﻿using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KoiCareSystem.Data.DBContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<KoiFish> KoiFishes { get; set; }

    public virtual DbSet<KoiGrowthLog> KoiGrowthLogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Pond> Ponds { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WaterParameter> WaterParameters { get; set; }

    public virtual DbSet<WaterParameterLimit> WaterParameterLimits { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Title).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.Blogs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<KoiFish>(entity =>
        {
            entity.HasKey(e => e.FishId);

            entity.Property(e => e.FishName).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Pond).WithMany(p => p.KoiFishes).HasForeignKey(d => d.PondId);

            entity.HasOne(d => d.User).WithMany(p => p.KoiFishes).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<KoiGrowthLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.Property(e => e.RecommendedFoodAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Fish).WithMany(p => p.KoiGrowthLogs).HasForeignKey(d => d.FishId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders).HasForeignKey(d => d.StatusId);

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusName).IsRequired();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Pond>(entity =>
        {
            entity.Property(e => e.Depth).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PondName).IsRequired();
            entity.Property(e => e.PumpCapacity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Volume).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Ponds).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).IsRequired();
            entity.Property(e => e.ProductType).IsRequired();

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<WaterParameter>(entity =>
        {
            entity.HasKey(e => e.ParameterId);

            entity.Property(e => e.No2)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("NO2");
            entity.Property(e => e.No3)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("NO3");
            entity.Property(e => e.O2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ph)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PH");
            entity.Property(e => e.Po4)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PO4");
            entity.Property(e => e.Salinity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Temperature).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Pond).WithMany(p => p.WaterParameters).HasForeignKey(d => d.PondId);
        });

        modelBuilder.Entity<WaterParameterLimit>(entity =>
        {
            entity.HasKey(e => e.ParameterId);

            entity.Property(e => e.MaxValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ParameterName).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}