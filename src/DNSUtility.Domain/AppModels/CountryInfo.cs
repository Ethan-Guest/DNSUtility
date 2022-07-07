namespace DNSUtility.Domain.AppModels;

public class CountryInfo
{
    public CountryInfo(string language, string country)
    {
        Language = language;
        Country = country;
    }

    public string Language { get; set; }
    public string Country { get; set; }
}