using System.Globalization;
using System.Net;
using CsvHelper;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.Parsers;

public class NameserverCsvParser : IParser
{
    /// <summary>
    ///     Parse a CSV file.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerable<Nameserver> Parse(string path, string userCountry)
    {
        // Create the webclient to read from URL
        var client = new WebClient();
        var stream = client.OpenRead(path);

        // Read the file
        using var reader = new StreamReader(stream);

        // Convert CSV to list of Nameserver using CsvHelper
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Nameserver>().ToList().Where(n =>
            !string.IsNullOrEmpty(n.CountryCode) && !string.IsNullOrEmpty(n.Name) &&
            !string.IsNullOrEmpty(n.IpAddress) && n.CountryCode == userCountry && !n.IpAddress.Contains(":"));
    }
}