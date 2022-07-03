using CsvHelper.Configuration.Attributes;
using ReactiveUI;

namespace DNSUtility.Domain;

public class Nameserver
{
    private string _country;

    private bool _dnssec;

    private string _ipAddress;

    private string _name;

    private decimal _reliability;

    private long _totalPing;

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

    [Ignore]
    public long TotalPing { get; set; }
}