namespace EComApi.Models;

public class ShopItemCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<ShopItem> ShopItems { get; set; } = new List<ShopItem>();
}