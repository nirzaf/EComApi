using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EComApi.Models;

namespace EComApi.Data;

public class EComDbContext(DbContextOptions<EComDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ShopItemCategory> ShopItemCategories { get; set; }
    public DbSet<ShopItem> ShopItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationship between ShopItem and ShopItemCategory
        modelBuilder.Entity<ShopItem>()
            .HasMany(si => si.Categories)
            .WithMany(sic => sic.ShopItems);

        // Configure Order -> Customer relationship
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        // Configure OrderItem relationships
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.ShopItem)
            .WithMany(si => si.OrderItems)
            .HasForeignKey(oi => oi.ShopItemId);

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Categories
        modelBuilder.Entity<ShopItemCategory>().HasData(
            new ShopItemCategory { Id = 1, Title = "Electronics", Description = "Electronic devices and gadgets" },
            new ShopItemCategory { Id = 2, Title = "Clothing", Description = "Fashion and apparel" },
            new ShopItemCategory { Id = 3, Title = "Books", Description = "Books and literature" }
        );

        // Seed Shop Items
        modelBuilder.Entity<ShopItem>().HasData(
            new ShopItem { Id = 1, Title = "Laptop", Description = "High-performance laptop", Price = 999.99f },
            new ShopItem { Id = 2, Title = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99f },
            new ShopItem { Id = 3, Title = "Programming Book", Description = "Learn programming fundamentals", Price = 49.99f }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com" },
            new Customer { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com" }
        );

        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, CustomerId = 1 },
            new Order { Id = 2, CustomerId = 2 }
        );

        // Seed Order Items
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ShopItemId = 1, Quantity = 1 },
            new OrderItem { Id = 2, OrderId = 1, ShopItemId = 3, Quantity = 2 },
            new OrderItem { Id = 3, OrderId = 2, ShopItemId = 2, Quantity = 3 }
        );
    }
}