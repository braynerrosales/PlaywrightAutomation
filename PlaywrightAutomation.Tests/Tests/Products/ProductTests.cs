using System.Text.RegularExpressions;
using PlaywrightAutomation.Tests.Fixtures;
using PlaywrightAutomation.Tests.TestData;

namespace PlaywrightAutomation.Tests.Tests.Products;

[TestFixture]
[Category("Products")]
public class ProductTests : BaseTest
{
    [Test]
    [Category("Smoke")]
    public async Task Products_WhenPageLoads_ShouldDisplayProductCatalog()
    {
        var product = ProductData.BlueTop;

        await ProductsPage.GoToAsync();

        await Expect(Page).ToHaveURLAsync(new Regex("/products$"));
        await Expect(ProductsPage.AllProductsTitle).ToBeVisibleAsync();
        await Expect(ProductsPage.ProductCards.First).ToBeVisibleAsync();
        Assert.That(await ProductsPage.ProductCards.CountAsync(), Is.GreaterThan(1));

        await Expect(ProductsPage.ProductCard(product.Name)).ToBeVisibleAsync();
        await Expect(ProductsPage.ProductCardPrice(product.Name)).ToHaveTextAsync(product.FormattedPrice);
    }

    [Test]
    [Category("Regression")]
    public async Task Products_SearchExistingProduct_ShouldDisplayMatchingResults()
    {
        await ProductsPage.GoToAsync();

        await ProductsPage.SearchAsync(ProductData.SearchTerm);

        await Expect(Page).ToHaveURLAsync(new Regex($"search={ProductData.SearchTerm}"));
        await Expect(ProductsPage.SearchedProductsTitle).ToBeVisibleAsync();
        await Expect(ProductsPage.ProductCard(ProductData.ExpectedSearchResult)).ToBeVisibleAsync();

        var resultNames = await ProductsPage.ProductNames.AllInnerTextsAsync();
        Assert.That(resultNames, Is.Not.Empty);
        Assert.That(resultNames, Has.All.Contains(ProductData.SearchTerm).IgnoreCase,
            "Every search result should match the search term");
    }

    [Test]
    [Category("Regression")]
    public async Task Products_OpenProduct_ShouldDisplayProductDetails()
    {
        var product = ProductData.BlueTop;
        await ProductsPage.GoToAsync();

        await ProductsPage.OpenProductDetailsAsync(product.Name);

        await Expect(Page).ToHaveURLAsync(new Regex($"/product_details/{product.Id}$"));
        await Expect(ProductDetailsPage.Name).ToHaveTextAsync(product.Name);
        await Expect(ProductDetailsPage.Category).ToHaveTextAsync($"Category: {product.Category}");
        await Expect(ProductDetailsPage.Price).ToHaveTextAsync(product.FormattedPrice);
        await Expect(ProductDetailsPage.Availability).ToHaveTextAsync("Availability: In Stock");
        await Expect(ProductDetailsPage.Condition).ToHaveTextAsync("Condition: New");
        await Expect(ProductDetailsPage.Brand).ToHaveTextAsync($"Brand: {product.Brand}");
        await Expect(ProductDetailsPage.QuantityInput).ToHaveValueAsync("1");
    }
}
