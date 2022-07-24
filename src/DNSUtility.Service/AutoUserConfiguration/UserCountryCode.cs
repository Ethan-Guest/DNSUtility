using System.Globalization;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.AutoUserConfiguration;

/// <summary>
///     A service for retrieving information about the users country
/// </summary>
public class UserCountryCode : ICountryInfo
{
    public CountryInfo GetCountryCode()
    {
        // Collect information about the culture of the users location
        var culture = CultureInfo.CurrentCulture.Name;

        // Parse the language and country into a variable
        var language = culture.Remove(culture.Length - 3);
        var country = culture.Substring(culture.Length - 2);

        return new CountryInfo(language, country);
    }
}