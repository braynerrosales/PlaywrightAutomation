namespace PlaywrightAutomation.Tests.Models;

public record Product(
    int Id,
    string Name,
    int Price,
    string Category,
    string Brand)
{
    public string FormattedPrice => FormatPrice(Price);

    public static string FormatPrice(int amount) => $"Rs. {amount}";
}
