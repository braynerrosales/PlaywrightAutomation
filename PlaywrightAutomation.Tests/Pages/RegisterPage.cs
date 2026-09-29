using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Pages;

public class RegisterPage
{
    private readonly IPage _page;

    public RegisterPage(IPage page)
    {
        _page = page;
    }

    // Step 1: "New User Signup!" form (shown on /login).
    private ILocator SignupNameInput => _page.GetByTestId("signup-name");

    private ILocator SignupEmailInput => _page.GetByTestId("signup-email");

    private ILocator SignupButton => _page.GetByTestId("signup-button");

    public ILocator SignupErrorMessage => _page.Locator("form[action='/signup'] p");

    // Step 2: "Enter Account Information" form.
    public ILocator AccountInformationTitle =>
        _page.GetByRole(AriaRole.Heading, new() { Name = "Enter Account Information" });

    private ILocator TitleMrRadio => _page.GetByLabel("Mr.", new() { Exact = true });

    private ILocator PasswordInput => _page.GetByTestId("password");

    private ILocator FirstNameInput => _page.GetByTestId("first_name");

    private ILocator LastNameInput => _page.GetByTestId("last_name");

    private ILocator CompanyInput => _page.GetByTestId("company");

    private ILocator AddressInput => _page.GetByTestId("address");

    private ILocator CountrySelect => _page.GetByTestId("country");

    private ILocator StateInput => _page.GetByTestId("state");

    private ILocator CityInput => _page.GetByTestId("city");

    private ILocator ZipcodeInput => _page.GetByTestId("zipcode");

    private ILocator MobileNumberInput => _page.GetByTestId("mobile_number");

    private ILocator CreateAccountButton => _page.GetByTestId("create-account");

    // Result pages.
    public ILocator AccountCreatedTitle => _page.GetByTestId("account-created");

    public ILocator AccountDeletedTitle => _page.GetByTestId("account-deleted");

    private ILocator ContinueButton => _page.GetByTestId("continue-button");

    public async Task GoToAsync()
    {
        await _page.GotoAsync("/login");
    }

    public async Task StartSignupAsync(string name, string email)
    {
        await SignupNameInput.FillAsync(name);
        await SignupEmailInput.FillAsync(email);
        await SignupButton.ClickAsync();
    }

    public async Task CompleteAccountInformationAsync(TestUser user)
    {
        await TitleMrRadio.CheckAsync();
        await PasswordInput.FillAsync(user.Password);
        await FirstNameInput.FillAsync(user.FirstName);
        await LastNameInput.FillAsync(user.LastName);
        await CompanyInput.FillAsync(user.Company);
        await AddressInput.FillAsync(user.Address);
        await CountrySelect.SelectOptionAsync(user.Country);
        await StateInput.FillAsync(user.State);
        await CityInput.FillAsync(user.City);
        await ZipcodeInput.FillAsync(user.Zipcode);
        await MobileNumberInput.FillAsync(user.MobileNumber);
        await CreateAccountButton.ClickAsync();
    }

    public async Task ContinueAsync()
    {
        await ContinueButton.ClickAsync();
    }
}
