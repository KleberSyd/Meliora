using Meliora.Domain.Models.CookiesKristen;

namespace Meliora.Services.CookiesKristen.Dto;

public class ReportDto
{
    public int NumberOrders { get; set; }
    public int TotalCookies { get; set; }
    public List<Mixin> MostPopular { get; set; }
}