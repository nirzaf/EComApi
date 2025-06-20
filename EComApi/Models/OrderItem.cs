namespace EComApi.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    // Foreign keys
    public int ShopItemId { get; set; }
    public int OrderId { get; set; }
    
    // Navigation properties
    public ShopItem ShopItem { get; set; } = null!;
    public Order Order { get; set; } = null!;
}