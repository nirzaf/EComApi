namespace EComApi.Models;

/// <summary>
/// Represents an order in the e-commerce system
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for the order
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Foreign key to the customer who placed the order
    /// </summary>
    public int CustomerId { get; set; }
    
    /// <summary>
    /// Navigation property to the customer
    /// </summary>
    public virtual Customer Customer { get; set; } = null!;
    
    /// <summary>
    /// Navigation property for items in this order
    /// </summary>
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    /// <summary>
    /// Date and time when the order was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}