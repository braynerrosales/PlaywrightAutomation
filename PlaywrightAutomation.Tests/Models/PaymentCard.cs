namespace PlaywrightAutomation.Tests.Models;

public record PaymentCard(
    string NameOnCard,
    string Number,
    string Cvc,
    string ExpiryMonth,
    string ExpiryYear);
