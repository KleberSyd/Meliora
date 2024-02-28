namespace Meliora.Domain.Models.CookiesKristen;

public class Mixin
{
    protected Mixin()
    {
        
    }
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockQuantity { get; set; }
}