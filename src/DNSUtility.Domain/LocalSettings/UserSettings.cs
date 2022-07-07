namespace DNSUtility.Domain.UserSettings;

/// <summary>
/// The users settings that are used to initialize the app.
/// </summary>
public class UserSettings
{
    public UserSettings(string country, string language)
    {
        Country = country;
        Language = language;
    }

    /// <summary>
    /// Two letter country code. e.g. US, CA, MX
    /// </summary>
    public string Country { get; set; }

    
    /// <summary>
    /// Language abbreviation. e.g. EN, ES, FR,
    /// </summary>
    public string Language { get; set; }
}