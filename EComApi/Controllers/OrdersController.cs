using EComApi.Data;
using EComApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComApi.Controllers;

/// <summary>
/// Controller for managing orders
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController(EComDbContext context) : ControllerBase
{
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>List of orders</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.ShopItem)
            .ToListAsync();
    }

    /// <summary>
    /// Get a specific order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Order details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.ShopItem)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="orderDto">Order data</param>
    /// <returns>Created order</returns>
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(OrderDto orderDto)
    {
        // Verify customer exists
        var customer = await context.Customers.FindAsync(orderDto.CustomerId);
        if (customer == null)
        {
            return BadRequest("Customer not found.");
        }

        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            CreatedAt = DateTime.UtcNow
        };

        // Add order items
        foreach (var itemDto in orderDto.Items)
        {
            var shopItem = await context.ShopItems.FindAsync(itemDto.ShopItemId);
            if (shopItem == null)
            {
                return BadRequest($"Shop item with ID {itemDto.ShopItemId} not found.");
            }

            var orderItem = new OrderItem
            {
                ShopItemId = itemDto.ShopItemId,
                Quantity = itemDto.Quantity
            };

            order.Items.Add(orderItem);
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    /// <summary>
    /// Update an existing order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="orderDto">Updated order data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, OrderDto orderDto)
    {
        var order = await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // Verify customer exists
        var customer = await context.Customers.FindAsync(orderDto.CustomerId);
        if (customer == null)
        {
            return BadRequest("Customer not found.");
        }

        order.CustomerId = orderDto.CustomerId;

        // Remove existing order items
        context.OrderItems.RemoveRange(order.Items);

        // Add new order items
        order.Items.Clear();
        foreach (var itemDto in orderDto.Items)
        {
            var shopItem = await context.ShopItems.FindAsync(itemDto.ShopItemId);
            if (shopItem == null)
            {
                return BadRequest($"Shop item with ID {itemDto.ShopItemId} not found.");
            }

            var orderItem = new OrderItem
            {
                ShopItemId = itemDto.ShopItemId,
                Quantity = itemDto.Quantity,
                OrderId = order.Id
            };

            order.Items.Add(orderItem);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Check if an order exists
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>True if order exists</returns>
    private bool OrderExists(int id)
    {
        return context.Orders.Any(e => e.Id == id);
    }
}

/// <summary>
/// DTO for creating and updating orders
/// </summary>
public class OrderDto
{
    public int CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}

/// <summary>
/// DTO for order items
/// </summary>
public class OrderItemDto
{
    public int ShopItemId { get; set; }
    public int Quantity { get; set; }
}