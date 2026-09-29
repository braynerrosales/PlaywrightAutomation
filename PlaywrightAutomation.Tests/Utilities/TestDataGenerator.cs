using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.Utilities;

public static class TestDataGenerator
{
    public static string GenerateEmail()
    {
        return $"qa.{Guid.NewGuid():N}@example.com";
    }

    public static string GeneratePassword()
    {
        return $"Qa#{Guid.NewGuid():N}"[..16];
    }

    public static TestUser CreateUser()
    {
        var suffix = Guid.NewGuid().ToString("N")[..6];

        return new TestUser
        {
            Name = $"QA User {suffix}",
            Email = GenerateEmail(),
            Password = GeneratePassword(),
            FirstName = "QA",
            LastName = $"Tester{suffix}",
            Company = "Test Company",
            Address = "123 Automation Street",
            Country = "Canada",
            State = "Ontario",
            City = "Toronto",
            Zipcode = "M5V2T6",
            MobileNumber = "5550000000"
        };
    }
}
