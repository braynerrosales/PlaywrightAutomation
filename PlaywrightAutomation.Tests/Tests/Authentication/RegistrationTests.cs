using System.Text.RegularExpressions;
using PlaywrightAutomation.Tests.Fixtures;
using PlaywrightAutomation.Tests.TestData;
using PlaywrightAutomation.Tests.Utilities;

namespace PlaywrightAutomation.Tests.Tests.Authentication;

[TestFixture]
[Category("Authentication")]
public class RegistrationTests : BaseTest
{
    [Test]
    [Category("Smoke")]
    public async Task Register_WithNewEmail_ShouldCreateAccountAndLogIn()
    {
        var user = TestDataGenerator.CreateUser();
        RegisterForCleanup(user);
        await RegisterPage.GoToAsync();

        await RegisterPage.StartSignupAsync(user.Name, user.Email);
        await Expect(RegisterPage.AccountInformationTitle).ToBeVisibleAsync();
        await RegisterPage.CompleteAccountInformationAsync(user);

        await Expect(RegisterPage.AccountCreatedTitle).ToHaveTextAsync("Account Created!");
        await RegisterPage.ContinueAsync();
        await Expect(Header.LoggedInUser).ToHaveTextAsync($"Logged in as {user.Name}");
    }

    [Test]
    [Category("Regression")]
    public async Task Register_WithExistingEmail_ShouldDisplayError()
    {
        var existingUser = await CreateRegisteredUserAsync();
        await RegisterPage.GoToAsync();

        await RegisterPage.StartSignupAsync("Another Name", existingUser.Email);

        await Expect(RegisterPage.SignupErrorMessage).ToHaveTextAsync(UserData.ExistingEmailError);
        await Expect(RegisterPage.AccountInformationTitle).ToBeHiddenAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/signup$"));
    }

    [Test]
    [Category("Regression")]
    public async Task DeleteAccount_WhenLoggedIn_ShouldPreventFurtherLogins()
    {
        var user = await CreateRegisteredUserAsync();
        await LoginPage.GoToAsync();
        await LoginPage.LoginAsync(user.Email, user.Password);
        await Expect(Header.LoggedInUser).ToBeVisibleAsync();

        await Header.DeleteAccountAsync();

        await Expect(RegisterPage.AccountDeletedTitle).ToHaveTextAsync("Account Deleted!");
        await RegisterPage.ContinueAsync();
        await Expect(Header.LoggedInUser).ToBeHiddenAsync();

        await LoginPage.GoToAsync();
        await LoginPage.LoginAsync(user.Email, user.Password);
        await Expect(LoginPage.ErrorMessage).ToHaveTextAsync(UserData.InvalidCredentialsError);
    }
}
