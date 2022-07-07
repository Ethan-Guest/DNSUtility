using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.AutoUserConfiguration;

public interface ICountryInfo
{
    CountryInfo GetCountryCode();
}