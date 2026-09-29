using System.Text.RegularExpressions;
using PlaywrightAutomation.Tests.Fixtures;
using PlaywrightAutomation.Tests.Models;
using PlaywrightAutomation.Tests.TestData;

namespace PlaywrightAutomation.Tests.Tests.Checkout;

[TestFixture]
[Category("Checkout")]
public class CheckoutTests : AuthenticatedTest
{
    [Test]
    [Category("Smoke")]
    public async Task Checkout_WithAuthenticatedUser_ShouldOpenCheckout()
    {
        await OpenCheckoutWithProductsAsync(ProductData.BlueTop);

        await Expect(Page).ToHaveURLAsync(new Regex("/checkout$"));
        await Expect(CheckoutPage.AddressDetailsTitle).ToBeVisibleAsync();
        await Expect(CheckoutPage.ReviewOrderTitle).ToBeVisibleAsync();

        foreach (var address in new[] { CheckoutPage.DeliveryAddress, CheckoutPage.BillingAddress })
        {
            await Expect(address).ToContainTextAsync($"{CurrentUser.FirstName} {CurrentUser.LastName}");
            await Expect(address).ToContainTextAsync(CurrentUser.Address);
            await Expect(address).ToContainTextAsync(CurrentUser.City);
            await Expect(address).ToContainTextAsync(CurrentUser.Country);
        }
    }

    [Test]
    [Category("Regression")]
    public async Task Checkout_WhenOpened_ShouldDisplayOrderSummary()
    {
        var products = new[] { ProductData.BlueTop, ProductData.MenTshirt };

        await OpenCheckoutWithProductsAsync(products);

        await Expect(CheckoutPage.Items.ItemRows).ToHaveCountAsync(products.Length);
        foreach (var product in products)
        {
            await Expect(CheckoutPage.Items.ItemPrice(product.Name)).ToHaveTextAsync(product.FormattedPrice);
            await Expect(CheckoutPage.Items.ItemQuantity(product.Name)).ToHaveTextAsync("1");
            await Expect(CheckoutPage.Items.ItemTotal(product.Name)).ToHaveTextAsync(product.FormattedPrice);
        }

        var expectedTotal = Product.FormatPrice(products.Sum(product => product.Price));
        await Expect(CheckoutPage.TotalAmount).ToHaveTextAsync(expectedTotal);
    }

    [Test]
    [Category("Smoke")]
    public async Task Checkout_WithValidTestData_ShouldCompleteOrder()
    {
        await OpenCheckoutWithProductsAsync(ProductData.BlueTop);

        await CheckoutPage.PlaceOrderAsync("Automated test order - please ignore.");
        await Expect(Page).ToHaveURLAsync(new Regex("/payment$"));
        await PaymentPage.PayAsync(PaymentData.TestCard);

        await Expect(PaymentPage.OrderPlacedTitle).ToHaveTextAsync("Order Placed!");
        await Expect(PaymentPage.OrderConfirmationMessage).ToBeVisibleAsync();
    }

    private async Task OpenCheckoutWithProductsAsync(params Product[] products)
    {
        await ProductsPage.GoToAsync();
        await ProductsPage.AddToCartAsync(products);
        await CartPage.GoToAsync();
        await CartPage.ProceedToCheckoutAsync();
    }
}
