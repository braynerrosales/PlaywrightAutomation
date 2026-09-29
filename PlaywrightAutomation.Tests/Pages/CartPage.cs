using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Pages.Components;

namespace PlaywrightAutomation.Tests.Pages;

public class CartPage
{
    private readonly IPage _page;

    public CartPage(IPage page)
    {
        _page = page;
        Items = new OrderItemsTable(page);
    }

    public OrderItemsTable Items { get; }

    public ILocator EmptyCartMessage => _page.GetByText("Cart is empty!");

    private ILocator ProceedToCheckoutButton => _page.GetByText("Proceed To Checkout");

    public async Task GoToAsync()
    {
        await _page.GotoAsync("/view_cart");
    }

    public async Task RemoveItemAsync(string productName)
    {
        await Items.ItemDeleteButton(productName).ClickAsync();
    }

    public async Task ProceedToCheckoutAsync()
    {
        await ProceedToCheckoutButton.ClickAsync();
    }
}
