using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.Parsers;

/// <summary>
///     An interface for parsing different files
/// </summary>
public interface IParser
{
    IEnumerable<Nameserver> Parse(string path, string userCountry);
}