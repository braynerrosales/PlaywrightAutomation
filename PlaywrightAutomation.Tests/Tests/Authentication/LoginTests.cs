using System.Text.RegularExpressions;
using PlaywrightAutomation.Tests.Fixtures;
using PlaywrightAutomation.Tests.Pages;
using PlaywrightAutomation.Tests.TestData;
using PlaywrightAutomation.Tests.Utilities;

namespace PlaywrightAutomation.Tests.Tests.Authentication;

[TestFixture]
[Category("Authentication")]
public class LoginTests : BaseTest
{
    [Test]
    [Category("Smoke")]
    public async Task Login_WithValidCredentials_ShouldAuthenticateUser()
    {
        var user = await CreateRegisteredUserAsync();
        await LoginPage.GoToAsync();
        await Expect(LoginPage.LoginFormTitle).ToBeVisibleAsync();

        await LoginPage.LoginAsync(user.Email, user.Password);

        await Expect(Page).ToHaveURLAsync(new Regex($"^{Regex.Escape(Settings.BaseUrl)}/?$"));
        await Expect(Header.LoggedInUser).ToHaveTextAsync($"Logged in as {user.Name}");
        await Expect(Header.LogoutLink).ToBeVisibleAsync();
    }

    [Test]
    [Category("Regression")]
    public async Task Login_WithInvalidPassword_ShouldDisplayError()
    {
        var user = await CreateRegisteredUserAsync();
        await LoginPage.GoToAsync();

        await LoginPage.LoginAsync(user.Email, UserData.InvalidPassword);

        await Expect(LoginPage.ErrorMessage).ToHaveTextAsync(UserData.InvalidCredentialsError);
        await Expect(Page).ToHaveURLAsync(new Regex("/login$"));
        await Expect(Header.LoggedInUser).ToBeHiddenAsync();
        await Expect(Header.SignupLoginLink).ToBeVisibleAsync();
    }

    [Test]
    [Category("Regression")]
    public async Task Login_WithoutRequiredData_ShouldNotAuthenticate()
    {
        await LoginPage.GoToAsync();

        await LoginPage.LoginAsync(email: string.Empty, password: string.Empty);

        var emailMissing = await LoginPage.IsValueMissingAsync(LoginPage.EmailInput);
        var passwordMissing = await LoginPage.IsValueMissingAsync(LoginPage.PasswordInput);

        Assert.Multiple(() =>
        {
            Assert.That(emailMissing, Is.True, "Email should be flagged as a required field");
            Assert.That(passwordMissing, Is.True, "Password should be flagged as a required field");
        });
        await Expect(Page).ToHaveURLAsync(new Regex("/login$"));
        await Expect(LoginPage.ErrorMessage).ToBeHiddenAsync();
        await Expect(Header.LoggedInUser).ToBeHiddenAsync();
    }
}
