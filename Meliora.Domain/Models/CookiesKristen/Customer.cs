namespace Meliora.Domain.Models.CookiesKristen;

public class Customer
{
    protected Customer()
    {
        
    }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Order> Orders { get; set; }
}