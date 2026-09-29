using Microsoft.Playwright;

namespace PlaywrightAutomation.Tests.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    // The login and signup forms share the "Email Address" placeholder, so the site's data-qa ids are used.
    public ILocator EmailInput => _page.GetByTestId("login-email");

    public ILocator PasswordInput => _page.GetByTestId("login-password");

    private ILocator LoginButton => _page.GetByTestId("login-button");

    public ILocator LoginFormTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Login to your account" });

    public ILocator ErrorMessage => _page.Locator("form[action='/login'] p");

    public async Task GoToAsync()
    {
        await _page.GotoAsync("/login");
    }

    public async Task LoginAsync(string email, string password)
    {
        await EmailInput.FillAsync(email);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    /// <summary>Returns true when the browser flags the field as required but empty (HTML5 validation).</summary>
    public Task<bool> IsValueMissingAsync(ILocator field) =>
        field.EvaluateAsync<bool>("element => element.validity.valueMissing");
}
