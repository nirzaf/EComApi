namespace EComApi.Models;

/// <summary>
/// Represents a category for shop items
/// </summary>
public class ShopItemCategory
{
    /// <summary>
    /// Unique identifier for the category
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Title of the category
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the category
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Navigation property for shop items in this category
    /// </summary>
    public virtual ICollection<ShopItem> ShopItems { get; set; } = new List<ShopItem>();
}