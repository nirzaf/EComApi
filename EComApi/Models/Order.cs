namespace EComApi.Models;

public class Order
{
    public int Id { get; set; }
    
    // Foreign key
    public int CustomerId { get; set; }
    
    // Navigation properties
    public Customer? Customer { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}