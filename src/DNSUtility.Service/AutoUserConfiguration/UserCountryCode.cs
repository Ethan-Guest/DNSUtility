using System.Globalization;

namespace DNSUtility.Service.AutoUserConfiguration;

public class UserCountryCode : ICountryInfo
{
    public Tuple<string, string> GetCountryCode()
    {
        string culture = CultureInfo.CurrentCulture.Name;
        string langCountry = culture; // Returns something like "en-US"

        string[] substrings = langCountry.Split('-');
        var language = substrings[0];
        var country = substrings[1];

        var languageAndCountry = new Tuple<string, string>(language, country);
        return languageAndCountry;
    }
}