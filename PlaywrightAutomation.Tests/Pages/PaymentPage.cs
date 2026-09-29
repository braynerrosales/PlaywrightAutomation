using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Pages;

public class PaymentPage
{
    private readonly IPage _page;

    public PaymentPage(IPage page)
    {
        _page = page;
    }

    private ILocator NameOnCardInput => _page.GetByTestId("name-on-card");

    private ILocator CardNumberInput => _page.GetByTestId("card-number");

    private ILocator CvcInput => _page.GetByTestId("cvc");

    private ILocator ExpiryMonthInput => _page.GetByTestId("expiry-month");

    private ILocator ExpiryYearInput => _page.GetByTestId("expiry-year");

    private ILocator PayAndConfirmButton => _page.GetByTestId("pay-button");

    public ILocator OrderPlacedTitle => _page.GetByTestId("order-placed");

    public ILocator OrderConfirmationMessage => _page.GetByText("Congratulations! Your order has been confirmed!");

    public async Task PayAsync(PaymentCard card)
    {
        await NameOnCardInput.FillAsync(card.NameOnCard);
        await CardNumberInput.FillAsync(card.Number);
        await CvcInput.FillAsync(card.Cvc);
        await ExpiryMonthInput.FillAsync(card.ExpiryMonth);
        await ExpiryYearInput.FillAsync(card.ExpiryYear);
        await PayAndConfirmButton.ClickAsync();
    }
}
