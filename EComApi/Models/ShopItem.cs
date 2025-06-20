namespace EComApi.Models;

/// <summary>
/// Represents a shop item (product) in the e-commerce system
/// </summary>
public class ShopItem
{
    /// <summary>
    /// Unique identifier for the shop item
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Title of the shop item
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the shop item
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Price of the shop item
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Navigation property for categories this item belongs to
    /// </summary>
    public virtual ICollection<ShopItemCategory> Categories { get; set; } = new List<ShopItemCategory>();
    
    /// <summary>
    /// Navigation property for order items that reference this shop item
    /// </summary>
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}