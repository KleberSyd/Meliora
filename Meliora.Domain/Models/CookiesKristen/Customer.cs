using System.Diagnostics.CodeAnalysis;

namespace Meliora.Domain.Models.CookiesKristen;

public class Customer
{
    // EF Core requires a parameterless constructor
    protected Customer()
    {
        
    }

    [SetsRequiredMembers]
    public Customer(string name, string email) : this()
    {
        Name = name;
        Email = email;
    }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Order> Orders { get; set; }
}