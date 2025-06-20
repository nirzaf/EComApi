using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using EComApi.Models;

namespace TestEComApi;

public class CustomersControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetCustomers_ReturnsSuccessAndCustomers()
    {
        // Act
        var response = await _client.GetAsync("/api/customers");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetCustomer_WithValidId_ReturnsCustomer()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(customer);
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public async Task GetCustomer_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/999");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostCustomer_WithValidData_CreatesCustomer()
    {
        // Arrange
        var newCustomer = new Customer
        {
            Name = "Test",
            Surname = "User",
            Email = "test.user@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/customers", newCustomer);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        
        var createdCustomer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(createdCustomer);
        Assert.Equal(newCustomer.Name, createdCustomer.Name);
        Assert.Equal(newCustomer.Email, createdCustomer.Email);
    }
}

public class ShopItemCategoriesControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetShopItemCategories_ReturnsSuccessAndCategories()
    {
        // Act
        var response = await _client.GetAsync("/api/shopitemcategories");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetShopItemCategory_WithValidId_ReturnsCategory()
    {
        // Act
        var response = await _client.GetAsync("/api/shopitemcategories/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var category = await response.Content.ReadFromJsonAsync<ShopItemCategory>();
        Assert.NotNull(category);
        Assert.Equal(1, category.Id);
    }

    [Fact]
    public async Task PostShopItemCategory_WithValidData_CreatesCategory()
    {
        // Arrange
        var newCategory = new ShopItemCategory
        {
            Title = "Test Category",
            Description = "Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/shopitemcategories", newCategory);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}

public class ShopItemsControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetShopItems_ReturnsSuccessAndItems()
    {
        // Act
        var response = await _client.GetAsync("/api/shopitems");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetShopItem_WithValidId_ReturnsItem()
    {
        // Act
        var response = await _client.GetAsync("/api/shopitems/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<ShopItem>();
        Assert.NotNull(item);
        Assert.Equal(1, item.Id);
    }

    [Fact]
    public async Task PostShopItem_WithValidData_CreatesItem()
    {
        // Arrange
        var newItem = new ShopItem
        {
            Title = "Test Item",
            Description = "Test Description",
            Price = 29.99f
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/shopitems", newItem);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}

public class OrdersControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetOrders_ReturnsSuccessAndOrders()
    {
        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetOrder_WithValidId_ReturnsOrder()
    {
        // Act
        var response = await _client.GetAsync("/api/orders/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<Order>();
        Assert.NotNull(order);
        Assert.Equal(1, order.Id);
    }

    [Fact]
    public async Task PostOrder_WithValidData_CreatesOrder()
    {
        // Arrange
        var newOrder = new Order
        {
            CustomerId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", newOrder);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}