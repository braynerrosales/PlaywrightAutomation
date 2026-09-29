using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Pages.Components;

namespace PlaywrightAutomation.Tests.Pages;

public class CheckoutPage
{
    private readonly IPage _page;

    public CheckoutPage(IPage page)
    {
        _page = page;
        Items = new OrderItemsTable(page);
    }

    public OrderItemsTable Items { get; }

    public ILocator AddressDetailsTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Address Details" });

    public ILocator ReviewOrderTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Review Your Order" });

    public ILocator DeliveryAddress => _page.Locator("#address_delivery");

    public ILocator BillingAddress => _page.Locator("#address_invoice");

    public ILocator TotalAmount =>
        _page.Locator("#cart_info tbody tr")
            .Filter(new() { HasText = "Total Amount" })
            .Locator(".cart_total_price");

    private ILocator CommentInput => _page.Locator("textarea[name='message']");

    private ILocator PlaceOrderButton => _page.GetByRole(AriaRole.Link, new() { Name = "Place Order" });

    public async Task PlaceOrderAsync(string comment)
    {
        await CommentInput.FillAsync(comment);
        await PlaceOrderButton.ClickAsync();
    }
}
