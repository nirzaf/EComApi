using EComApi.Data;
using EComApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComApi.Controllers;

/// <summary>
/// Controller for managing customers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController(EComDbContext context) : ControllerBase
{
    /// <summary>
    /// Get all customers
    /// </summary>
    /// <returns>List of customers</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await context.Customers.ToListAsync();
    }

    /// <summary>
    /// Get a specific customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="customer">Customer data</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        // Check if email already exists
        if (await context.Customers.AnyAsync(c => c.Email == customer.Email))
        {
            return BadRequest("A customer with this email already exists.");
        }

        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="customer">Updated customer data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        // Check if email already exists for another customer
        if (await context.Customers.AnyAsync(c => c.Email == customer.Email && c.Id != id))
        {
            return BadRequest("A customer with this email already exists.");
        }

        context.Entry(customer).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Check if a customer exists
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>True if customer exists</returns>
    private bool CustomerExists(int id)
    {
        return context.Customers.Any(e => e.Id == id);
    }
}