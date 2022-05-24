using CsvHelper.Configuration.Attributes;
using ReactiveUI;

namespace DNSUtility.Domain;

public class Nameserver : ReactiveObject
{
    private string _country;

    private bool _dnssec;

    private string _ipAddress;

    private string _name;

    private decimal _reliability;

    private long _totalPing;

    [Name("ip_address")]
    public string IpAddress
    {
        get => _ipAddress;
        set => this.RaiseAndSetIfChanged(ref _ipAddress, value);
    }

    [Name("as_org")]
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    [Name("country_code")]
    public string Country
    {
        get => _country;
        set => this.RaiseAndSetIfChanged(ref _country, value);
    }

    [Name("dnssec")]
    public bool Dnssec
    {
        get => _dnssec;
        set => this.RaiseAndSetIfChanged(ref _dnssec, value);
    }

    [Name("reliability")]
    public decimal Reliability
    {
        get => _reliability;
        set => this.RaiseAndSetIfChanged(ref _reliability, value);
    }

    [Ignore]
    public long TotalPing
    {
        get => _totalPing;
        set => this.RaiseAndSetIfChanged(ref _totalPing, value);
    }
}