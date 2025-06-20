using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EComApi.Data;
using EComApi.Models;

namespace EComApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopItemsController(EComDbContext context) : ControllerBase
{
    // GET: api/shopitems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShopItem>>> GetShopItems()
    {
        return await context.ShopItems
            .Include(si => si.Categories)
            .Include(si => si.OrderItems)
            .ToListAsync();
    }

    // GET: api/shopitems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ShopItem>> GetShopItem(int id)
    {
        var shopItem = await context.ShopItems
            .Include(si => si.Categories)
            .Include(si => si.OrderItems)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (shopItem == null)
        {
            return NotFound();
        }

        return shopItem;
    }

    // POST: api/shopitems
    [HttpPost]
    public async Task<ActionResult<ShopItem>> PostShopItem(ShopItem shopItem)
    {
        context.ShopItems.Add(shopItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShopItem), new { id = shopItem.Id }, shopItem);
    }

    // PUT: api/shopitems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShopItem(int id, ShopItem shopItem)
    {
        if (id != shopItem.Id)
        {
            return BadRequest();
        }

        context.Entry(shopItem).State = EntityState.Modified;

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
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/shopitems/5
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

    private bool ShopItemExists(int id)
    {
        return context.ShopItems.Any(e => e.Id == id);
    }
}