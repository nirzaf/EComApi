using EComApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EComApi.Data;

/// <summary>
/// Database context for the e-commerce application
/// </summary>
public class EComDbContext : DbContext
{
    public EComDbContext(DbContextOptions<EComDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for customers
    /// </summary>
    public DbSet<Customer> Customers { get; set; }
    
    /// <summary>
    /// DbSet for shop item categories
    /// </summary>
    public DbSet<ShopItemCategory> ShopItemCategories { get; set; }
    
    /// <summary>
    /// DbSet for shop items
    /// </summary>
    public DbSet<ShopItem> ShopItems { get; set; }
    
    /// <summary>
    /// DbSet for orders
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    
    /// <summary>
    /// DbSet for order items
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Configure entity relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Surname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure ShopItemCategory entity
        modelBuilder.Entity<ShopItemCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        // Configure ShopItem entity
        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        // Configure many-to-many relationship between ShopItem and ShopItemCategory
        modelBuilder.Entity<ShopItem>()
            .HasMany(e => e.Categories)
            .WithMany(e => e.ShopItems)
            .UsingEntity("ShopItemCategoryMapping");

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Customer)
                  .WithMany(e => e.Orders)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithMany(e => e.Items)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.ShopItem)
                  .WithMany(e => e.OrderItems)
                  .HasForeignKey(e => e.ShopItemId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Quantity).IsRequired();
        });
    }
}