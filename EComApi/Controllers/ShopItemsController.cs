using EComApi.Data;
using EComApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComApi.Controllers;

/// <summary>
/// Controller for managing shop items
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ShopItemsController(EComDbContext context) : ControllerBase
{
    /// <summary>
    /// Get all shop items
    /// </summary>
    /// <returns>List of shop items</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShopItem>>> GetShopItems()
    {
        return await context.ShopItems
            .Include(s => s.Categories)
            .ToListAsync();
    }

    /// <summary>
    /// Get a specific shop item by ID
    /// </summary>
    /// <param name="id">Shop item ID</param>
    /// <returns>Shop item details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ShopItem>> GetShopItem(int id)
    {
        var shopItem = await context.ShopItems
            .Include(s => s.Categories)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shopItem == null)
        {
            return NotFound();
        }

        return shopItem;
    }

    /// <summary>
    /// Create a new shop item
    /// </summary>
    /// <param name="shopItemDto">Shop item data with category IDs</param>
    /// <returns>Created shop item</returns>
    [HttpPost]
    public async Task<ActionResult<ShopItem>> PostShopItem(ShopItemDto shopItemDto)
    {
        var shopItem = new ShopItem
        {
            Title = shopItemDto.Title,
            Description = shopItemDto.Description,
            Price = shopItemDto.Price
        };

        // Add categories if provided
        if (shopItemDto.CategoryIds != null && shopItemDto.CategoryIds.Any())
        {
            var categories = await context.ShopItemCategories
                .Where(c => shopItemDto.CategoryIds.Contains(c.Id))
                .ToListAsync();
            
            foreach (var category in categories)
            {
                shopItem.Categories.Add(category);
            }
        }

        context.ShopItems.Add(shopItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShopItem), new { id = shopItem.Id }, shopItem);
    }

    /// <summary>
    /// Update an existing shop item
    /// </summary>
    /// <param name="id">Shop item ID</param>
    /// <param name="shopItemDto">Updated shop item data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShopItem(int id, ShopItemDto shopItemDto)
    {
        var shopItem = await context.ShopItems
            .Include(s => s.Categories)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shopItem == null)
        {
            return NotFound();
        }

        shopItem.Title = shopItemDto.Title;
        shopItem.Description = shopItemDto.Description;
        shopItem.Price = shopItemDto.Price;

        // Update categories
        shopItem.Categories.Clear();
        if (shopItemDto.CategoryIds != null && shopItemDto.CategoryIds.Any())
        {
            var categories = await context.ShopItemCategories
                .Where(c => shopItemDto.CategoryIds.Contains(c.Id))
                .ToListAsync();
            
            foreach (var category in categories)
            {
                shopItem.Categories.Add(category);
            }
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShopItemExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a shop item
    /// </summary>
    /// <param name="id">Shop item ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShopItem(int id)
    {
        var shopItem = await context.ShopItems.FindAsync(id);
        if (shopItem == null)
        {
            return NotFound();
        }

        context.ShopItems.Remove(shopItem);
        await context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Check if a shop item exists
    /// </summary>
    /// <param name="id">Shop item ID</param>
    /// <returns>True if shop item exists</returns>
    private bool ShopItemExists(int id)
    {
        return context.ShopItems.Any(e => e.Id == id);
    }
}

/// <summary>
/// DTO for creating and updating shop items
/// </summary>
public class ShopItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<int>? CategoryIds { get; set; }
}