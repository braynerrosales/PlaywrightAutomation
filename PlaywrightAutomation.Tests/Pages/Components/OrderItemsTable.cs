using Microsoft.Playwright;

namespace PlaywrightAutomation.Tests.Pages.Components;

/// <summary>Product table rendered both on the cart page and on the checkout "Review Your Order" section.</summary>
public class OrderItemsTable
{
    private readonly IPage _page;

    public OrderItemsTable(IPage page)
    {
        _page = page;
    }

    // Product rows are the only rows with a "product-{id}" id; this skips the checkout "Total Amount" row.
    public ILocator ItemRows => _page.Locator("#cart_info tbody tr[id^='product-']");

    public ILocator ItemRow(string productName) =>
        ItemRows.Filter(new()
        {
            Has = _page.GetByRole(AriaRole.Link, new() { Name = productName, Exact = true })
        });

    public ILocator ItemPrice(string productName) => ItemRow(productName).Locator(".cart_price");

    public ILocator ItemQuantity(string productName) => ItemRow(productName).Locator(".cart_quantity");

    public ILocator ItemTotal(string productName) => ItemRow(productName).Locator(".cart_total");

    public ILocator ItemDeleteButton(string productName) => ItemRow(productName).Locator(".cart_quantity_delete");
}
