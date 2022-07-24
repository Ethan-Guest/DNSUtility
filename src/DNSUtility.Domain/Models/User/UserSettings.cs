using System.Net.NetworkInformation;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Domain.UserModels;

/// <summary>
///     The users settings that are used to initialize the app.
/// </summary>
public class UserSettings
{
    public UserSettings(CountryInfo countryInfo, NetworkInterface[] networkInterfaces,
        NetworkInterface? activeInterface)
    {
        Country = countryInfo.Country;
        Language = countryInfo.Language;
        if (countryInfo.Country == string.Empty) Country = "US";
        if (countryInfo.Language == string.Empty) Language = "en";

        // Create the network adapters class
        NetworkAdapters = new NetworkAdapters(networkInterfaces, activeInterface);
    }

    /// <summary>
    ///     Two letter country code. e.g. US, CA, MX
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    ///     Language abbreviation. e.g. EN, ES, FR,
    /// </summary>
    public string Language { get; set; }

    public NetworkAdapters NetworkAdapters { get; set; }
}