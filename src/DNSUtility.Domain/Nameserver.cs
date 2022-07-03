using CsvHelper.Configuration.Attributes;
using ReactiveUI;

namespace DNSUtility.Domain;

public class Nameserver
{
    // The nameserver class is a standard model class with CsvHelper name attributes
    // that map the headers of a csv file into their corresponding data types
    [Name("ip_address")]
    public string IpAddress { get; set; }

    [Name("as_org")]
    public string Name { get; set; }

    [Name("country_code")]
    public string Country { get; set; }

    [Name("dnssec")]
    public bool Dnssec { get; set; }

    [Name("reliability")]
    public decimal Reliability { get; set; }
    
}