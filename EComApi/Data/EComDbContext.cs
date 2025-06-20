using Microsoft.EntityFrameworkCore;
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
        // Seed Categories (10+ entries)
        modelBuilder.Entity<ShopItemCategory>().HasData(
            new ShopItemCategory { Id = 1, Title = "Electronics", Description = "Electronic devices and gadgets" },
            new ShopItemCategory { Id = 2, Title = "Clothing", Description = "Fashion and apparel" },
            new ShopItemCategory { Id = 3, Title = "Books", Description = "Books and literature" },
            new ShopItemCategory { Id = 4, Title = "Home & Garden", Description = "Home improvement and gardening supplies" },
            new ShopItemCategory { Id = 5, Title = "Sports & Outdoor", Description = "Sports equipment and outdoor gear" },
            new ShopItemCategory { Id = 6, Title = "Health & Beauty", Description = "Health, wellness and beauty products" },
            new ShopItemCategory { Id = 7, Title = "Toys & Games", Description = "Toys, games and entertainment" },
            new ShopItemCategory { Id = 8, Title = "Automotive", Description = "Car parts and automotive accessories" },
            new ShopItemCategory { Id = 9, Title = "Food & Beverages", Description = "Food, drinks and gourmet items" },
            new ShopItemCategory { Id = 10, Title = "Office Supplies", Description = "Office and business supplies" }
        );

        // Seed Shop Items (15+ entries)
        modelBuilder.Entity<ShopItem>().HasData(
            new ShopItem { Id = 1, Title = "Laptop", Description = "High-performance laptop", Price = 999.99f },
            new ShopItem { Id = 2, Title = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99f },
            new ShopItem { Id = 3, Title = "Programming Book", Description = "Learn programming fundamentals", Price = 49.99f },
            new ShopItem { Id = 4, Title = "Smartphone", Description = "Latest model smartphone with advanced features", Price = 799.99f },
            new ShopItem { Id = 5, Title = "Jeans", Description = "Premium quality denim jeans", Price = 89.99f },
            new ShopItem { Id = 6, Title = "Wireless Headphones", Description = "Noise-cancelling wireless headphones", Price = 299.99f },
            new ShopItem { Id = 7, Title = "Running Shoes", Description = "Professional running shoes", Price = 129.99f },
            new ShopItem { Id = 8, Title = "Coffee Maker", Description = "Automatic drip coffee maker", Price = 159.99f },
            new ShopItem { Id = 9, Title = "Yoga Mat", Description = "Premium quality yoga mat", Price = 39.99f },
            new ShopItem { Id = 10, Title = "Gaming Mouse", Description = "High-precision gaming mouse", Price = 79.99f },
            new ShopItem { Id = 11, Title = "Desk Chair", Description = "Ergonomic office desk chair", Price = 249.99f },
            new ShopItem { Id = 12, Title = "Water Bottle", Description = "Stainless steel water bottle", Price = 24.99f },
            new ShopItem { Id = 13, Title = "Tablet", Description = "10-inch tablet with stylus", Price = 449.99f },
            new ShopItem { Id = 14, Title = "Hoodie", Description = "Comfortable fleece hoodie", Price = 59.99f },
            new ShopItem { Id = 15, Title = "Cookbook", Description = "International cuisine cookbook", Price = 34.99f }
        );

        // Seed Customers (12+ entries)
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com" },
            new Customer { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com" },
            new Customer { Id = 3, Name = "Michael", Surname = "Johnson", Email = "michael.johnson@example.com" },
            new Customer { Id = 4, Name = "Emily", Surname = "Davis", Email = "emily.davis@example.com" },
            new Customer { Id = 5, Name = "William", Surname = "Brown", Email = "william.brown@example.com" },
            new Customer { Id = 6, Name = "Sarah", Surname = "Wilson", Email = "sarah.wilson@example.com" },
            new Customer { Id = 7, Name = "David", Surname = "Miller", Email = "david.miller@example.com" },
            new Customer { Id = 8, Name = "Lisa", Surname = "Moore", Email = "lisa.moore@example.com" },
            new Customer { Id = 9, Name = "Robert", Surname = "Taylor", Email = "robert.taylor@example.com" },
            new Customer { Id = 10, Name = "Amanda", Surname = "Anderson", Email = "amanda.anderson@example.com" },
            new Customer { Id = 11, Name = "Christopher", Surname = "Thomas", Email = "christopher.thomas@example.com" },
            new Customer { Id = 12, Name = "Jessica", Surname = "Jackson", Email = "jessica.jackson@example.com" }
        );

        // Seed Orders (12+ entries)
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, CustomerId = 1 },
            new Order { Id = 2, CustomerId = 2 },
            new Order { Id = 3, CustomerId = 3 },
            new Order { Id = 4, CustomerId = 4 },
            new Order { Id = 5, CustomerId = 5 },
            new Order { Id = 6, CustomerId = 6 },
            new Order { Id = 7, CustomerId = 7 },
            new Order { Id = 8, CustomerId = 8 },
            new Order { Id = 9, CustomerId = 9 },
            new Order { Id = 10, CustomerId = 10 },
            new Order { Id = 11, CustomerId = 11 },
            new Order { Id = 12, CustomerId = 12 }
        );

        // Seed Order Items (20+ entries)
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ShopItemId = 1, Quantity = 1 },
            new OrderItem { Id = 2, OrderId = 1, ShopItemId = 3, Quantity = 2 },
            new OrderItem { Id = 3, OrderId = 2, ShopItemId = 2, Quantity = 3 },
            new OrderItem { Id = 4, OrderId = 3, ShopItemId = 4, Quantity = 1 },
            new OrderItem { Id = 5, OrderId = 3, ShopItemId = 6, Quantity = 1 },
            new OrderItem { Id = 6, OrderId = 4, ShopItemId = 5, Quantity = 2 },
            new OrderItem { Id = 7, OrderId = 4, ShopItemId = 7, Quantity = 1 },
            new OrderItem { Id = 8, OrderId = 5, ShopItemId = 8, Quantity = 1 },
            new OrderItem { Id = 9, OrderId = 5, ShopItemId = 12, Quantity = 2 },
            new OrderItem { Id = 10, OrderId = 6, ShopItemId = 9, Quantity = 1 },
            new OrderItem { Id = 11, OrderId = 6, ShopItemId = 10, Quantity = 1 },
            new OrderItem { Id = 12, OrderId = 7, ShopItemId = 11, Quantity = 1 },
            new OrderItem { Id = 13, OrderId = 8, ShopItemId = 13, Quantity = 1 },
            new OrderItem { Id = 14, OrderId = 8, ShopItemId = 14, Quantity = 2 },
            new OrderItem { Id = 15, OrderId = 9, ShopItemId = 15, Quantity = 1 },
            new OrderItem { Id = 16, OrderId = 9, ShopItemId = 3, Quantity = 1 },
            new OrderItem { Id = 17, OrderId = 10, ShopItemId = 1, Quantity = 1 },
            new OrderItem { Id = 18, OrderId = 10, ShopItemId = 10, Quantity = 1 },
            new OrderItem { Id = 19, OrderId = 11, ShopItemId = 2, Quantity = 4 },
            new OrderItem { Id = 20, OrderId = 11, ShopItemId = 14, Quantity = 2 },
            new OrderItem { Id = 21, OrderId = 12, ShopItemId = 6, Quantity = 1 },
            new OrderItem { Id = 22, OrderId = 12, ShopItemId = 12, Quantity = 3 }
        );
    }
}