using Meliora.Domain.Models.CookiesKristen;

namespace Meliora.Services.CookiesKristen.Interfaces;

public interface IOrderService
{
    Task ProcessOrderAsync(Order order);
}