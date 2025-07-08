using EComApi.Data;
using EComApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComApi.Controllers;

/// <summary>
/// Controller for managing shop item categories
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ShopItemCategoriesController(EComDbContext context) : ControllerBase
{
    /// <summary>
    /// Get all shop item categories
    /// </summary>
    /// <returns>List of categories</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShopItemCategory>>> GetShopItemCategories()
    {
        return await context.ShopItemCategories.ToListAsync();
    }

    /// <summary>
    /// Get a specific category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Category details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ShopItemCategory>> GetShopItemCategory(int id)
    {
        var category = await context.ShopItemCategories
            .Include(c => c.ShopItems)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="category">Category data</param>
    /// <returns>Created category</returns>
    [HttpPost]
    public async Task<ActionResult<ShopItemCategory>> PostShopItemCategory(ShopItemCategory category)
    {
        context.ShopItemCategories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShopItemCategory), new { id = category.Id }, category);
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="category">Updated category data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShopItemCategory(int id, ShopItemCategory category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        context.Entry(category).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShopItemCategoryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShopItemCategory(int id)
    {
        var category = await context.ShopItemCategories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        context.ShopItemCategories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Check if a category exists
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>True if category exists</returns>
    private bool ShopItemCategoryExists(int id)
    {
        return context.ShopItemCategories.Any(e => e.Id == id);
    }
}