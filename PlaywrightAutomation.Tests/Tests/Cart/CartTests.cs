using PlaywrightAutomation.Tests.Fixtures;
using PlaywrightAutomation.Tests.TestData;

namespace PlaywrightAutomation.Tests.Tests.Cart;

[TestFixture]
[Category("Cart")]
public class CartTests : BaseTest
{
    [Test]
    [Category("Smoke")]
    public async Task Cart_AddProduct_ShouldDisplayProductInCart()
    {
        var product = ProductData.BlueTop;
        await ProductsPage.GoToAsync();

        await ProductsPage.AddToCartAsync(product);
        await CartPage.GoToAsync();

        await Expect(CartPage.Items.ItemRows).ToHaveCountAsync(1);
        await Expect(CartPage.Items.ItemRow(product.Name)).ToBeVisibleAsync();
        await Expect(CartPage.Items.ItemPrice(product.Name)).ToHaveTextAsync(product.FormattedPrice);
        await Expect(CartPage.Items.ItemQuantity(product.Name)).ToHaveTextAsync("1");
        await Expect(CartPage.Items.ItemTotal(product.Name)).ToHaveTextAsync(product.FormattedPrice);
    }

    [Test]
    [Category("Regression")]
    public async Task Cart_AddMultipleProducts_ShouldDisplayAllItems()
    {
        var products = new[] { ProductData.BlueTop, ProductData.MenTshirt };
        await ProductsPage.GoToAsync();

        await ProductsPage.AddToCartAsync(products);
        await CartPage.GoToAsync();

        await Expect(CartPage.Items.ItemRows).ToHaveCountAsync(products.Length);
        foreach (var product in products)
        {
            await Expect(CartPage.Items.ItemPrice(product.Name)).ToHaveTextAsync(product.FormattedPrice);
            await Expect(CartPage.Items.ItemQuantity(product.Name)).ToHaveTextAsync("1");
            await Expect(CartPage.Items.ItemTotal(product.Name)).ToHaveTextAsync(product.FormattedPrice);
        }
    }

    [Test]
    [Category("Regression")]
    public async Task Cart_RemoveProduct_ShouldRemoveItemFromCart()
    {
        var product = ProductData.BlueTop;
        await ProductsPage.GoToAsync();
        await ProductsPage.AddToCartAsync(product);
        await CartPage.GoToAsync();
        await Expect(CartPage.Items.ItemRow(product.Name)).ToBeVisibleAsync();

        await CartPage.RemoveItemAsync(product.Name);

        await Expect(CartPage.Items.ItemRow(product.Name)).ToHaveCountAsync(0);
        await Expect(CartPage.EmptyCartMessage).ToBeVisibleAsync();

        await Page.ReloadAsync();
        await Expect(CartPage.Items.ItemRows).ToHaveCountAsync(0);
    }
}
