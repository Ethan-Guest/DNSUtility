using System.Globalization;
using System.Net;
using CsvHelper;
using DNSUtility.Domain;

namespace DNSUtility.Service.Parsers;

public class NameserverCsvParser : IParser
{
    /// <summary>
    ///     Parse a CSV file.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerable<Nameserver> Parse(string path)
    {
        // Create the webclient to read from URL
        var client = new WebClient();
        var stream = client.OpenRead(path);

        // Read the file
        using var reader = new StreamReader(stream);

        // Convert CSV to list of Nameserver using CsvHelper
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Nameserver>().ToList().Where(n =>
            !string.IsNullOrEmpty(n.Country) && !string.IsNullOrEmpty(n.Name) && !string.IsNullOrEmpty(n.IpAddress) &&
            n.Country == "ZA");
    }
}