namespace Meliora.Services.CookiesKristen.MailHogDependencies;

public class Item
{
    public required string id;
    public required Content Content { get; set; }
    public required From From { get; set; }
}