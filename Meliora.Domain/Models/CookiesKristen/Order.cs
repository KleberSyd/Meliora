using Meliora.Domain.Enum;

namespace Meliora.Domain.Models.CookiesKristen;

public class Order
{
    protected Order()
    {
        
    }
    public int Id { get; set; }
    public required Customer Customer { get; set; }
    public required List<Mixin> Mixins { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
}