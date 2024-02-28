using Meliora.Domain.Enum;
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

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Mixins)
            .Where(c => c.Status == OrderStatus.Pending || c.Status == OrderStatus.InPreparation)
            .ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderId)
    {
        var order = await context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null) {throw new Exception("Order not found.");}

        switch (order.Status)
        {
            case OrderStatus.Pending:
                order.Status = OrderStatus.InPreparation;
                break;
            case OrderStatus.InPreparation:
                order.Status = OrderStatus.Completed;
                await SendOrderReadyEmail(order);
                break;
            case OrderStatus.Completed:
                break;
            default:
                throw new Exception("Invalid order status.");
        }

        await context.SaveChangesAsync();
    }

    private async Task SendOrderReadyEmail(Order order)
    {
        await new MailHogService().SendEmailAsync(order.Customer.Email, $"order: {order.Id} it is done", 
            $"the order {order.Id} is READY \n " +
            $"Quantity: {order.Quantity} " +
            $"\nFlavors: {string.Join(", ", order.Mixins.Select(m => m.Name))} " +
            $"\nName: {order.Customer.Name}");
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}