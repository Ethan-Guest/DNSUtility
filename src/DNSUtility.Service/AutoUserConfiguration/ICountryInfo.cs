namespace DNSUtility.Service.AutoUserConfiguration;

public interface ICountryInfo
{
    Tuple<string, string> GetCountryCode();
}