using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;
using PlaywrightAutomation.Tests.Configuration;
using PlaywrightAutomation.Tests.Models;
using PlaywrightAutomation.Tests.Pages;
using PlaywrightAutomation.Tests.Pages.Components;
using PlaywrightAutomation.Tests.Utilities;

namespace PlaywrightAutomation.Tests.Fixtures;

public abstract class BaseTest : PageTest
{
    protected static TestSettings Settings => TestSettings.Current;

    // Third-party ad/consent/analytics hosts. They are not part of the application under test and
    // their overlays (e.g. Google vignette ads) can intercept clicks, so requests to them are aborted.
    private static readonly Regex ThirdPartyAdHosts = new(
        @"googlesyndication|doubleclick|googleadservices|adservice\.google|adtrafficquality|fundingchoicesmessages|google-analytics|googletagmanager",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private readonly List<TestUser> _usersToCleanUp = new();
    private AccountApiClient _accountApi = null!;

    protected HeaderComponent Header = null!;
    protected LoginPage LoginPage = null!;
    protected RegisterPage RegisterPage = null!;
    protected ProductsPage ProductsPage = null!;
    protected ProductDetailsPage ProductDetailsPage = null!;
    protected CartPage CartPage = null!;
    protected CheckoutPage CheckoutPage = null!;
    protected PaymentPage PaymentPage = null!;

    private static string VideoWorkDirectory => Path.Combine(Settings.ArtifactsDirectory, ".video-tmp");

    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = Settings.BaseUrl,
        ViewportSize = new() { Width = Settings.ViewportWidth, Height = Settings.ViewportHeight },
        RecordVideoDir = Settings.Video == ArtifactMode.Off ? null : VideoWorkDirectory,
        RecordVideoSize = Settings.Video == ArtifactMode.Off
            ? null
            : new() { Width = Settings.ViewportWidth, Height = Settings.ViewportHeight }
    };

    [SetUp]
    public async Task SetUpBaseAsync()
    {
        Playwright.Selectors.SetTestIdAttribute("data-qa");
        Context.SetDefaultTimeout(Settings.DefaultTimeout);

        if (Settings.BlockAds)
        {
            await Context.RouteAsync(ThirdPartyAdHosts, route => route.AbortAsync());
        }

        if (Settings.Trace != ArtifactMode.Off)
        {
            await Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.FullName,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        _accountApi = await AccountApiClient.CreateAsync(Playwright, Settings.BaseUrl);

        Header = new HeaderComponent(Page);
        LoginPage = new LoginPage(Page);
        RegisterPage = new RegisterPage(Page);
        ProductsPage = new ProductsPage(Page);
        ProductDetailsPage = new ProductDetailsPage(Page);
        CartPage = new CartPage(Page);
        CheckoutPage = new CheckoutPage(Page);
        PaymentPage = new PaymentPage(Page);
    }

    [TearDown]
    public async Task TearDownBaseAsync()
    {
        try
        {
            await SaveEvidenceAsync();
        }
        finally
        {
            foreach (var user in _usersToCleanUp)
            {
                await _accountApi.DeleteAccountAsync(user);
            }

            await _accountApi.DisposeAsync();
        }
    }

    /// <summary>Creates a new account through the API. It is deleted automatically after the test.</summary>
    protected async Task<TestUser> CreateRegisteredUserAsync()
    {
        var user = TestDataGenerator.CreateUser();
        await _accountApi.CreateAccountAsync(user);
        RegisterForCleanup(user);
        return user;
    }

    /// <summary>Marks an account created during the test (e.g. through the UI) for deletion after the test.</summary>
    protected void RegisterForCleanup(TestUser user)
    {
        _usersToCleanUp.Add(user);
    }

    private async Task SaveEvidenceAsync()
    {
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        var fileName = SanitizeFileName($"{TestContext.CurrentContext.Test.Name}.{BrowserName}");
        Directory.CreateDirectory(Settings.ArtifactsDirectory);

        if (ShouldKeep(Settings.Screenshot, failed))
        {
            var screenshotPath = Path.Combine(Settings.ArtifactsDirectory, $"{fileName}.png");
            await Page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });
            TestContext.AddTestAttachment(screenshotPath, "Screenshot");
        }

        if (Settings.Trace != ArtifactMode.Off)
        {
            var tracePath = ShouldKeep(Settings.Trace, failed)
                ? Path.Combine(Settings.ArtifactsDirectory, $"{fileName}.trace.zip")
                : null;
            await Context.Tracing.StopAsync(new() { Path = tracePath });

            if (tracePath is not null)
            {
                TestContext.AddTestAttachment(tracePath, "Playwright trace (open at trace.playwright.dev)");
            }
        }

        if (Settings.Video != ArtifactMode.Off && Page.Video is { } video)
        {
            // The video file is only finalized once the context is closed.
            await Context.CloseAsync();
            var recordedPath = await video.PathAsync();

            if (ShouldKeep(Settings.Video, failed))
            {
                var videoPath = Path.Combine(Settings.ArtifactsDirectory, $"{fileName}.webm");
                File.Move(recordedPath, videoPath, overwrite: true);
                TestContext.AddTestAttachment(videoPath, "Video");
            }
            else
            {
                File.Delete(recordedPath);
            }
        }
    }

    private static bool ShouldKeep(ArtifactMode mode, bool failed) =>
        mode == ArtifactMode.Always || (mode == ArtifactMode.OnFailure && failed);

    private static string SanitizeFileName(string name) =>
        string.Concat(name.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
}
