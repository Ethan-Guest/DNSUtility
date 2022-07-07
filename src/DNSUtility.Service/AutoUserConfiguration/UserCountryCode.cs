using System.Globalization;

namespace DNSUtility.Service.AutoUserConfiguration;

public class UserCountryCode : ICountryInfo
{
    public string GetCountryCode()
    {
        string culture = CultureInfo.CurrentCulture.Name;
        string country = culture;
        return country;
    }
}