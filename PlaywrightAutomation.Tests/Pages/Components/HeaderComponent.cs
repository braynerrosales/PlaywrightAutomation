using Microsoft.Playwright;

namespace PlaywrightAutomation.Tests.Pages.Components;

/// <summary>Top navigation bar shared by every page of the site.</summary>
public class HeaderComponent
{
    private readonly IPage _page;

    public HeaderComponent(IPage page)
    {
        _page = page;
    }

    public ILocator LoggedInUser => _page.GetByText("Logged in as");

    public ILocator SignupLoginLink => _page.GetByRole(AriaRole.Link, new() { Name = "Signup / Login" });

    public ILocator LogoutLink => _page.GetByRole(AriaRole.Link, new() { Name = "Logout" });

    private ILocator DeleteAccountLink => _page.GetByRole(AriaRole.Link, new() { Name = "Delete Account" });

    public async Task DeleteAccountAsync()
    {
        await DeleteAccountLink.ClickAsync();
    }
}
