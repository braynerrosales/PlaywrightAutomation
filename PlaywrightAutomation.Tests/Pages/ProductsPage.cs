using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Pages;

public class ProductsPage
{
    private readonly IPage _page;

    public ProductsPage(IPage page)
    {
        _page = page;
    }

    public ILocator AllProductsTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "All Products" });

    public ILocator SearchedProductsTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Searched Products" });

    public ILocator SearchInput => _page.GetByPlaceholder("Search Product");

    // Icon-only button without an accessible name; its id is the most stable hook.
    private ILocator SearchButton => _page.Locator("#submit_search");

    public ILocator ProductCards => _page.Locator(".features_items .product-image-wrapper");

    public ILocator ProductNames => ProductCards.Locator(".productinfo p");

    private ILocator AddedToCartDialog => _page.GetByRole(AriaRole.Heading, new() { Name = "Added!" });

    private ILocator ContinueShoppingButton => _page.GetByRole(AriaRole.Button, new() { Name = "Continue Shopping" });

    public ILocator ProductCard(string productName) =>
        ProductCards.Filter(new()
        {
            Has = _page.Locator(".productinfo").GetByText(productName, new() { Exact = true })
        });

    public ILocator ProductCardPrice(string productName) =>
        ProductCard(productName).Locator(".productinfo h2");

    public async Task GoToAsync()
    {
        await _page.GotoAsync("/products");
    }

    public async Task SearchAsync(string term)
    {
        await SearchInput.FillAsync(term);
        await SearchButton.ClickAsync();
    }

    public async Task OpenProductDetailsAsync(string productName)
    {
        await ProductCard(productName).GetByRole(AriaRole.Link, new() { Name = "View Product" }).ClickAsync();
    }

    public async Task AddToCartAsync(params Product[] products)
    {
        foreach (var product in products)
        {
            await ProductCard(product.Name).Locator(".productinfo").GetByText("Add to cart").ClickAsync();
            await AddedToCartDialog.WaitForAsync();
            await ContinueShoppingButton.ClickAsync();
            await AddedToCartDialog.WaitForAsync(new() { State = WaitForSelectorState.Hidden });
        }
    }
}
