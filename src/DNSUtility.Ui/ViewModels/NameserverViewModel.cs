using System.Collections.Generic;
using DNSUtility.Domain;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DNSUtility.Ui.ViewModels;

public class NameserverViewModel : ViewModelBase
{
    private string _country;

    private bool _dnssec;

    private string _ipAddress;

    private string _name;

    private decimal _reliability;

    private List<ushort> _pings;
    
    private ushort _totalPing;

    public NameserverViewModel(Nameserver nameserver)
    {
        IpAddress = nameserver.IpAddress;
        Name = nameserver.Name;
        Country = nameserver.Country;
        Dnssec = nameserver.Dnssec;
        Reliability = nameserver.Reliability;
        TotalPing = 0;
        Pings = new List<ushort>();
    }
    public string IpAddress
    {
        get => _ipAddress;
        set => this.RaiseAndSetIfChanged(ref _ipAddress, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Country
    {
        get => _country;
        set => this.RaiseAndSetIfChanged(ref _country, value);
    }

    public bool Dnssec
    {
        get => _dnssec;
        set => this.RaiseAndSetIfChanged(ref _dnssec, value);
    }

    public decimal Reliability
    {
        get => _reliability;
        set => this.RaiseAndSetIfChanged(ref _reliability, value);
    }

    public List<ushort> Pings 
    {
        get => _pings;
        set => this.RaiseAndSetIfChanged(ref _pings, value);
    }
    public ushort TotalPing
    {
        get => _totalPing;
        set => this.RaiseAndSetIfChanged(ref _totalPing, value);
    }
}