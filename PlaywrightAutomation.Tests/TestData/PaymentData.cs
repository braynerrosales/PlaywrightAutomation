using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.TestData;

public static class PaymentData
{
    // Public test card number (not a real card). The practice site does not charge anything.
    public static readonly PaymentCard TestCard =
        new("QA Automation", "4111111111111111", "123", "12", "2030");
}
