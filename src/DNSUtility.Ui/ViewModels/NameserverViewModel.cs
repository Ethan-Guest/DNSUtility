using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;
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
    
    private ushort _latestPing;

    private ushort _averagePing;

    public NameserverViewModel(Nameserver nameserver)
    {
        // Initialize nameserver properties
        IpAddress = nameserver.IpAddress;
        Name = nameserver.Name;
        Country = nameserver.Country;
        Dnssec = nameserver.Dnssec;
        Reliability = nameserver.Reliability;
        AveragePing = 0;
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
    public ushort LatestPing
    {
        get => _latestPing;
        set => this.RaiseAndSetIfChanged(ref _latestPing, value);
    }
    public ushort AveragePing
    {
        get => _averagePing;
        set => this.RaiseAndSetIfChanged(ref _averagePing, value);
    }

    // TODO move to service
    // A method for calculating the average ping of the name server
    public void CalculateAveragePing()
    {
        foreach (var ping in Pings)
        {
            AveragePing += ping;
        }
        AveragePing = (ushort)(AveragePing / Pings.Count);
    }
}