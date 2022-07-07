using System.Globalization;
using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.AutoUserConfiguration;

public class UserCountryCode : ICountryInfo
{
    public CountryInfo GetCountryCode()
    {
        var culture = CultureInfo.CurrentCulture.Name;
        var language = culture.Remove(culture.Length - 3);
        var country = culture.Substring(culture.Length - 2);
        
        return new CountryInfo(language, country);
    }
}