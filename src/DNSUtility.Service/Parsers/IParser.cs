using DNSUtility.Domain;

namespace DNSUtility.Service.Parsers;

public interface IParser
{
    IEnumerable<Nameserver> Parse(string path);
}