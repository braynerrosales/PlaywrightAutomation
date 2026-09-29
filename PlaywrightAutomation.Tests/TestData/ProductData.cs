using PlaywrightAutomation.Tests.Models;

namespace PlaywrightAutomation.Tests.TestData;

public static class ProductData
{
    public static readonly Product BlueTop =
        new(1, "Blue Top", 500, "Women > Tops", "Polo");

    public static readonly Product MenTshirt =
        new(2, "Men Tshirt", 400, "Men > Tshirts", "H&M");

    public const string SearchTerm = "Jeans";

    public const string ExpectedSearchResult = "Soft Stretch Jeans";
}
