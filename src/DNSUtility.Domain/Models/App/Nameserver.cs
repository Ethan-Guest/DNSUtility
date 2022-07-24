using CsvHelper.Configuration.Attributes;

namespace DNSUtility.Domain.AppModels;

/// <summary>
///     The nameserver class is a standard model class with CsvHelper name attributes
///     that map the headers of a csv file into their corresponding data types
/// </summary>
public class Nameserver
{
    // The IP Address of the nameserver
    [Name("ip_address")] public string IpAddress { get; set; }

    // The name of the nameserver
    [Name("as_org")] public string Name { get; set; }

    // The country code of the nameserver
    [Name("country_code")] public string CountryCode { get; set; }

    // A bool storing whether the nameserver has DNS Security enabled
    [Name("dnssec")] public bool Dnssec { get; set; }

    // The reliability score of the nameserver (0.0 - 100.0)
    [Name("reliability")] public decimal Reliability { get; set; }
}