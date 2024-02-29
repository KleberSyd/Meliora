using Meliora.Domain.Enum;
using Meliora.Domain.Models.CookiesKristen;
using Meliora.Repository.Context;
using Meliora.Services.CookiesKristen.Dto;
using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Services.CookiesKristen;

public class OrderService(CookieKristenDbContext context) : IOrderService, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Processes an order asynchronously.
    /// </summary>
    /// <param name="order">The order to process.</param>
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

    /// <summary>
    /// Retrieves a list of orders asynchronously.
    /// </summary>
    /// <returns>A list of orders.</returns>
    public async Task<List<Order>?> GetOrdersAsync()
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Mixins)
            .Where(c => c.Status == OrderStatus.Pending || c.Status == OrderStatus.InPreparation)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a report of orders asynchronously.
    /// </summary>
    /// <returns>A report containing the number of orders, total number of cookies, and the most popular mixins.</returns>
    public async Task<ReportDto?> GetOrdersReportAsync()
    {
        return new ReportDto
        {
            NumberOrders = await context.Orders.CountAsync(),
            TotalCookies = await context.Orders.SumAsync(o => o.Quantity),
            MostPopular = await context.Orders
                .SelectMany(o => o.Mixins)
                .GroupBy(m => m.Id)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First())
                .Take(3)
                .ToListAsync()
        };
    }

    /// <summary>
    /// Updates the status of an order asynchronously.
    /// </summary>
    /// <param name="orderId">The ID of the order to update.</param>
    public async Task UpdateOrderStatusAsync(int orderId)
    {
        var order = await context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null) { throw new Exception("Order not found."); }

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

    /// <summary>
    /// Sends an email notification when an order is ready.
    /// </summary>
    /// <param name="order">The order that is ready.</param>
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
