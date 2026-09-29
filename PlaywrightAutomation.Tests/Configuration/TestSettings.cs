using System.Globalization;
using System.Text.RegularExpressions;

namespace PlaywrightAutomation.Tests.Configuration;

/// <summary>When an evidence artifact (trace, screenshot, video) is kept.</summary>
public enum ArtifactMode
{
    Off,
    OnFailure,
    Always
}

/// <summary>
/// Framework settings read from the &lt;TestRunParameters&gt; section of the active .runsettings file.
/// Every value can be overridden with an environment variable named E2E_ + the parameter name in
/// UPPER_SNAKE_CASE (e.g. BaseUrl → E2E_BASE_URL, Trace → E2E_TRACE).
/// Browser launch options (browser, headless, slowMo, channel) live in the &lt;Playwright&gt; section instead.
/// </summary>
public sealed class TestSettings
{
    private static readonly Lazy<TestSettings> Instance = new(Load);

    public static TestSettings Current => Instance.Value;

    public required string BaseUrl { get; init; }

    public required int ViewportWidth { get; init; }

    public required int ViewportHeight { get; init; }

    /// <summary>Maximum time in milliseconds for actions and navigations (click, fill, goto...).</summary>
    public required float DefaultTimeout { get; init; }

    /// <summary>Abort requests to third-party ad/consent/analytics hosts.</summary>
    public required bool BlockAds { get; init; }

    public required ArtifactMode Screenshot { get; init; }

    public required ArtifactMode Trace { get; init; }

    public required ArtifactMode Video { get; init; }

    /// <summary>Absolute path, or relative to the test output directory.</summary>
    public required string ArtifactsDirectory { get; init; }

    private static TestSettings Load() => new()
    {
        BaseUrl = Read(nameof(BaseUrl), "https://automationexercise.com").TrimEnd('/'),
        ViewportWidth = ReadInt(nameof(ViewportWidth), 1366),
        ViewportHeight = ReadInt(nameof(ViewportHeight), 900),
        DefaultTimeout = ReadInt(nameof(DefaultTimeout), 30000),
        BlockAds = ReadBool(nameof(BlockAds), true),
        Screenshot = ReadMode(nameof(Screenshot), ArtifactMode.OnFailure),
        Trace = ReadMode(nameof(Trace), ArtifactMode.OnFailure),
        Video = ReadMode(nameof(Video), ArtifactMode.Off),
        ArtifactsDirectory = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            Read(nameof(ArtifactsDirectory), "playwright-artifacts"))
    };

    private static string Read(string name, string defaultValue)
    {
        var environmentValue = Environment.GetEnvironmentVariable(ToEnvironmentVariableName(name));

        return !string.IsNullOrWhiteSpace(environmentValue)
            ? environmentValue.Trim()
            : TestContext.Parameters.Get(name, defaultValue);
    }

    private static int ReadInt(string name, int defaultValue)
    {
        var value = Read(name, defaultValue.ToString(CultureInfo.InvariantCulture));

        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
            ? result
            : throw InvalidValue(name, value, "an integer");
    }

    private static bool ReadBool(string name, bool defaultValue)
    {
        var value = Read(name, defaultValue.ToString());

        return value.ToLowerInvariant() switch
        {
            "true" or "1" or "yes" => true,
            "false" or "0" or "no" => false,
            _ => throw InvalidValue(name, value, "true or false")
        };
    }

    private static ArtifactMode ReadMode(string name, ArtifactMode defaultValue)
    {
        var value = Read(name, defaultValue.ToString());

        // Accepts "on-failure", "on_failure" and "OnFailure".
        return Enum.TryParse<ArtifactMode>(value.Replace("-", "").Replace("_", ""), ignoreCase: true, out var mode)
            ? mode
            : throw InvalidValue(name, value, "off, on-failure or always");
    }

    private static string ToEnvironmentVariableName(string name) =>
        "E2E_" + Regex.Replace(name, "(?<=[a-z])(?=[A-Z])", "_").ToUpperInvariant();

    private static InvalidOperationException InvalidValue(string name, string value, string expected) =>
        new($"Invalid test setting '{name}' = '{value}'. Expected {expected} " +
            $"(runsettings parameter '{name}' or environment variable '{ToEnvironmentVariableName(name)}').");
}
