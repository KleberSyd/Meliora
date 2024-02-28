using Meliora.Domain.Models.CookiesKristen;
using Meliora.Repository.Context;
using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Services.CookiesKristen;

public class OrderService(CookieKristenDbContext context) : IOrderService, IDisposable, IAsyncDisposable
{
    public async Task ProcessOrderAsync(Order order)
    {
        var existingOrder = await context.Orders
            .Include(o => o.Mixins)
            .AnyAsync(o =>
                    o.Customer.Id == order.Customer.Id &&
                    o.Quantity == order.Quantity &&
                    o.Mixins.Count == order.Mixins.Count && 
                    o.Mixins.All(m => order.Mixins.Select(om => om.Id).Contains(m.Id))
            );

        if (existingOrder)
        {
            throw new FailedToOrderCookieException("Duplicate orders with the same mix-ins are not allowed.");
        }

        if (order.Quantity < 12)
        {
            throw new FailedToOrderCookieException("An order must contain at least 12 units.");
        }

        var totalCookiesToday = await context.Orders
            .Where(o => o.Date == DateTime.Today)
            .SumAsync(o => o.Quantity);

        const int maxCookiesPerDay = (3 * 60) / 20 * 24; 

        if (totalCookiesToday + order.Quantity > maxCookiesPerDay)
        {
            throw new FailedToOrderCookieException("This order cannot be accommodated in today's production schedule.");
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        await context.DisposeAsync();
    }
}