namespace EComApi.Models;

public class ShopItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    
    // Navigation properties
    public ICollection<ShopItemCategory> Categories { get; set; } = new List<ShopItemCategory>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}