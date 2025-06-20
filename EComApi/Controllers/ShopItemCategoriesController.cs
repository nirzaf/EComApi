using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EComApi.Data;
using EComApi.Models;

namespace EComApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopItemCategoriesController(EComDbContext context) : ControllerBase
{
    // GET: api/shopitemcategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShopItemCategory>>> GetShopItemCategories()
    {
        return await context.ShopItemCategories.Include(c => c.ShopItems).ToListAsync();
    }

    // GET: api/shopitemcategories/5
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

    // POST: api/shopitemcategories
    [HttpPost]
    public async Task<ActionResult<ShopItemCategory>> PostShopItemCategory(ShopItemCategory category)
    {
        context.ShopItemCategories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShopItemCategory), new { id = category.Id }, category);
    }

    // PUT: api/shopitemcategories/5
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

    // DELETE: api/shopitemcategories/5
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

    private bool ShopItemCategoryExists(int id)
    {
        return context.ShopItemCategories.Any(e => e.Id == id);
    }
}