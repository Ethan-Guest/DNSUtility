namespace DNSUtility.Domain.UserSettings;

public class UserSettings
{
    public UserSettings(string country, string language)
    {
        Country = country;
        Language = language;
    }

    public string Country { get; set; }

    public string Language { get; set; }
}