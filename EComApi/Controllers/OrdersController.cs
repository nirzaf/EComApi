using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EComApi.Data;
using EComApi.Models;

namespace EComApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(EComDbContext context) : ControllerBase
{
    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.ShopItem)
            .ToListAsync();
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.ShopItem)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    // PUT: api/orders/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        context.Entry(order).State = EntityState.Modified;

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
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/orders/5
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

    private bool OrderExists(int id)
    {
        return context.Orders.Any(e => e.Id == id);
    }
}