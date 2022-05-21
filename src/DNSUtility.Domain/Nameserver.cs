using CsvHelper.Configuration.Attributes;

namespace DNSUtility.Domain;

public class Nameserver
{
    [Name("ip_address")] public string IpAddress { get; set; }

    [Name("as_org")] public string Name { get; set; }

    [Name("country_code")] public string Country { get; set; }

    [Name("dnssec")] public bool Dnssec { get; set; }

    [Name("reliability")] public decimal Reliability { get; set; }

    [Ignore] public long TotalPing { get; set; }
}