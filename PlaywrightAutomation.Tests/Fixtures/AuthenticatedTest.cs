using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Fixtures;

/// <summary>
/// Base class for tests that need a logged-in user. Every test gets its own freshly created
/// account (deleted afterwards), so tests never share state.
/// </summary>
public abstract class AuthenticatedTest : BaseTest
{
    protected TestUser CurrentUser = null!;

    [SetUp]
    public async Task LogInNewUserAsync()
    {
        CurrentUser = await CreateRegisteredUserAsync();

        await LoginPage.GoToAsync();
        await LoginPage.LoginAsync(CurrentUser.Email, CurrentUser.Password);
        await Expect(Header.LoggedInUser).ToBeVisibleAsync();
    }
}
