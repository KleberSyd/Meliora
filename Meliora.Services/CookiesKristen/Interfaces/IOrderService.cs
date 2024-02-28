using Meliora.Domain.Models.CookiesKristen;

namespace Meliora.Services.CookiesKristen.Interfaces;

public interface IOrderService
{
    Task ProcessOrderAsync(Order order);
    Task<List<Order>> GetOrdersAsync();
    Task UpdateOrderStatusAsync(int orderId);
}