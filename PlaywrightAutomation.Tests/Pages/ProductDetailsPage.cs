using Microsoft.Playwright;

namespace PlaywrightAutomation.Tests.Pages;

public class ProductDetailsPage
{
    private readonly IPage _page;

    public ProductDetailsPage(IPage page)
    {
        _page = page;
    }

    private ILocator ProductInformation => _page.Locator(".product-information");

    public ILocator Name => ProductInformation.GetByRole(AriaRole.Heading, new() { Level = 2 });

    public ILocator Category => ProductInformation.GetByText("Category:");

    public ILocator Price => ProductInformation.GetByText("Rs.");

    public ILocator Availability => InformationLine("Availability:");

    public ILocator Condition => InformationLine("Condition:");

    public ILocator Brand => InformationLine("Brand:");

    public ILocator QuantityInput => ProductInformation.GetByRole(AriaRole.Spinbutton);

    private ILocator InformationLine(string label) =>
        ProductInformation.Locator("p").Filter(new() { HasText = label });
}
