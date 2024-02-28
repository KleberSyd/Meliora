using System.Diagnostics.CodeAnalysis;
using Meliora.Domain.Enum;

namespace Meliora.Domain.Models.CookiesKristen;

public class Order
{
    //EF Core requires a parameterless constructor
    protected Order()
    {
        
    }

    [SetsRequiredMembers]
    public Order(Customer customer, int quantity, List<Mixin> mixins, OrderStatus status) : this()
    {
        Customer = customer;
        Quantity = quantity;
        Mixins = mixins;
        Status = status;
        Date = DateTime.Now;
    }

    public int Id { get; set; }
    public required Customer Customer { get; set; }
    public required List<Mixin> Mixins { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime Date { get; set; }
}