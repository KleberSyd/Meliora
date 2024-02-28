namespace Meliora.Domain.Models;

public class Mixin
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockQuantity { get; set; }
}