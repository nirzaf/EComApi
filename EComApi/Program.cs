
using EComApi.Data;
using EComApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<EComDbContext>(options =>
    options.UseInMemoryDatabase("EComApiDb"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

// Initialize database with test data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EComDbContext>();
    await SeedDatabase(context);
}

app.Run();

/// <summary>
/// Seed the database with initial test data
/// </summary>
/// <param name="context">Database context</param>
async Task SeedDatabase(EComDbContext context)
{
    // Ensure database is created
    await context.Database.EnsureCreatedAsync();

    // Check if data already exists
    if (context.Customers.Any())
    {
        return; // Database has been seeded
    }

    // Add sample customers
    var customers = new[]
    {
        new Customer { Name = "John", Surname = "Doe", Email = "john.doe@example.com" },
        new Customer { Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com" },
        new Customer { Name = "Bob", Surname = "Johnson", Email = "bob.johnson@example.com" }
    };
    context.Customers.AddRange(customers);
    await context.SaveChangesAsync();

    // Add sample categories
    var categories = new[]
    {
        new ShopItemCategory { Title = "Electronics", Description = "Electronic devices and gadgets" },
        new ShopItemCategory { Title = "Clothing", Description = "Apparel and fashion items" },
        new ShopItemCategory { Title = "Books", Description = "Books and educational materials" },
        new ShopItemCategory { Title = "Home & Garden", Description = "Home improvement and gardening supplies" }
    };
    context.ShopItemCategories.AddRange(categories);
    await context.SaveChangesAsync();

    // Add sample shop items
    var shopItems = new[]
    {
        new ShopItem { Title = "Smartphone", Description = "Latest model smartphone", Price = 699.99m },
        new ShopItem { Title = "Laptop", Description = "High-performance laptop", Price = 1299.99m },
        new ShopItem { Title = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m },
        new ShopItem { Title = "Jeans", Description = "Denim jeans", Price = 49.99m },
        new ShopItem { Title = "Programming Book", Description = "Learn programming fundamentals", Price = 39.99m }
    };
    context.ShopItems.AddRange(shopItems);
    await context.SaveChangesAsync();

    // Assign categories to shop items
    shopItems[0].Categories.Add(categories[0]); // Smartphone -> Electronics
    shopItems[1].Categories.Add(categories[0]); // Laptop -> Electronics
    shopItems[2].Categories.Add(categories[1]); // T-Shirt -> Clothing
    shopItems[3].Categories.Add(categories[1]); // Jeans -> Clothing
    shopItems[4].Categories.Add(categories[2]); // Programming Book -> Books
    
    await context.SaveChangesAsync();

    // Add sample orders
    var orders = new[]
    {
        new Order 
        { 
            CustomerId = customers[0].Id,
            Items = new List<OrderItem>
            {
                new OrderItem { ShopItemId = shopItems[0].Id, Quantity = 1 },
                new OrderItem { ShopItemId = shopItems[2].Id, Quantity = 2 }
            }
        },
        new Order 
        { 
            CustomerId = customers[1].Id,
            Items = new List<OrderItem>
            {
                new OrderItem { ShopItemId = shopItems[1].Id, Quantity = 1 }
            }
        }
    };
    context.Orders.AddRange(orders);
    await context.SaveChangesAsync();
}