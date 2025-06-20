namespace EComApi.Models;

/// <summary>
/// Represents a customer in the e-commerce system
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier for the customer
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Customer's first name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Customer's last name
    /// </summary>
    public string Surname { get; set; } = string.Empty;
    
    /// <summary>
    /// Customer's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Navigation property for orders placed by this customer
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}