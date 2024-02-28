using System.Diagnostics.CodeAnalysis;

namespace Meliora.Domain.Models.CookiesKristen;

public class Mixin
{
    // EF Core requires a parameterless constructor
    protected Mixin()
    {
        
    }

    [SetsRequiredMembers]
    public Mixin(string name) : this()
    {
        Name = name;
    }

    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockQuantity { get; set; }
}