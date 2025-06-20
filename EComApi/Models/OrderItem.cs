namespace EComApi.Models;

/// <summary>
/// Represents an item within an order
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Foreign key to the shop item
    /// </summary>
    public int ShopItemId { get; set; }
    
    /// <summary>
    /// Navigation property to the shop item
    /// </summary>
    public virtual ShopItem ShopItem { get; set; } = null!;
    
    /// <summary>
    /// Quantity of the shop item in this order
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Foreign key to the order
    /// </summary>
    public int OrderId { get; set; }
    
    /// <summary>
    /// Navigation property to the order
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}