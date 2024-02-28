namespace Meliora.Domain.Models;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Order> Orders { get; set; }
}