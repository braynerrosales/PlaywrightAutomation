using System.Text.Json;
using Microsoft.Playwright;
using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Utilities;

/// <summary>
/// Prepares and cleans up test accounts through the public Automation Exercise API,
/// so UI tests do not depend on each other to create users.
/// </summary>
public sealed class AccountApiClient : IAsyncDisposable
{
    private readonly IAPIRequestContext _request;

    private AccountApiClient(IAPIRequestContext request)
    {
        _request = request;
    }

    public static async Task<AccountApiClient> CreateAsync(IPlaywright playwright, string baseUrl)
    {
        var request = await playwright.APIRequest.NewContextAsync(new() { BaseURL = baseUrl });
        return new AccountApiClient(request);
    }

    public async Task CreateAccountAsync(TestUser user)
    {
        var form = _request.CreateFormData();
        form.Set("name", user.Name);
        form.Set("email", user.Email);
        form.Set("password", user.Password);
        form.Set("title", "Mr");
        form.Set("birth_date", "1");
        form.Set("birth_month", "1");
        form.Set("birth_year", "1990");
        form.Set("firstname", user.FirstName);
        form.Set("lastname", user.LastName);
        form.Set("company", user.Company);
        form.Set("address1", user.Address);
        form.Set("address2", string.Empty);
        form.Set("country", user.Country);
        form.Set("zipcode", user.Zipcode);
        form.Set("state", user.State);
        form.Set("city", user.City);
        form.Set("mobile_number", user.MobileNumber);

        var response = await _request.PostAsync("/api/createAccount", new() { Form = form });
        var code = await ReadResponseCodeAsync(response);

        if (code != 201)
        {
            throw new InvalidOperationException(
                $"Could not create test account '{user.Email}'. API response: {await response.TextAsync()}");
        }
    }

    /// <returns>true when the account existed and was deleted.</returns>
    public async Task<bool> DeleteAccountAsync(TestUser user)
    {
        var form = _request.CreateFormData();
        form.Set("email", user.Email);
        form.Set("password", user.Password);

        var response = await _request.DeleteAsync("/api/deleteAccount", new() { Form = form });
        return await ReadResponseCodeAsync(response) == 200;
    }

    public async ValueTask DisposeAsync()
    {
        await _request.DisposeAsync();
    }

    // The API always answers HTTP 200; the functional result lives in the JSON "responseCode".
    private static async Task<int> ReadResponseCodeAsync(IAPIResponse response)
    {
        using var json = JsonDocument.Parse(await response.TextAsync());
        return json.RootElement.GetProperty("responseCode").GetInt32();
    }
}
