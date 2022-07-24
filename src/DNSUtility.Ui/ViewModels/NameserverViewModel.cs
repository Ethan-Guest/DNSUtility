using System.Collections.Generic;
using System.Collections.ObjectModel;
using DNSUtility.Domain.AppModels;
using LiveChartsCore.Defaults;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverViewModel : ViewModelBase
{
    // Private backing fields
    private ushort _averagePing;
    private string _country;
    private bool _dnssec;
    private string _ipAddress;
    private ushort _latestPing;
    private string _name;
    private List<ushort> _pings;
    private decimal _reliability;
    private string _statusIcon;

    public NameserverViewModel(Nameserver nameserver)
    {
        // Initialize nameserver properties
        IpAddress = nameserver.IpAddress;
        Name = nameserver.Name;
        Country = nameserver.CountryCode;
        Dnssec = nameserver.Dnssec;
        Reliability = nameserver.Reliability;
        AveragePing = 0;
        Pings = new List<ushort>();
        ObservablePings = new ObservableCollection<ObservableValue>();

        // Set the default status icon color (grey)
        StatusIcon = "#858585";
    }

    // Properties
    //
    // An observable collection of pings
    public ObservableCollection<ObservableValue> ObservablePings { get; set; }

    // Live Properties
    //
    // The nameservers IP Address
    public string IpAddress
    {
        get => _ipAddress;
        set => this.RaiseAndSetIfChanged(ref _ipAddress, value);
    }

    // The nameservers name
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    // The nameservers country
    public string Country
    {
        get => _country;
        set => this.RaiseAndSetIfChanged(ref _country, value);
    }

    // The nameservers DNS security status (on / off)
    public bool Dnssec
    {
        get => _dnssec;
        set => this.RaiseAndSetIfChanged(ref _dnssec, value);
    }

    // The nameservers reliability (0.0 - 100.0)
    public decimal Reliability
    {
        get => _reliability;
        set => this.RaiseAndSetIfChanged(ref _reliability, value);
    }

    // A list to store the results of the benchmark test pings
    public List<ushort> Pings
    {
        get => _pings;
        set => this.RaiseAndSetIfChanged(ref _pings, value);
    }

    // The most recent ping from the benchmark test
    public ushort LatestPing
    {
        get => _latestPing;
        set => this.RaiseAndSetIfChanged(ref _latestPing, value);
    }

    // The average ping, to be calculated by taking the mean of all pings
    public ushort AveragePing
    {
        get => _averagePing;
        set => this.RaiseAndSetIfChanged(ref _averagePing, value);
    }

    // The status icon to be displayed in the UI
    public string StatusIcon
    {
        get => _statusIcon;
        set => this.RaiseAndSetIfChanged(ref _statusIcon, value);
    }

    // A method for calculating the average ping of the name server
    public void CalculateAveragePing()
    {
        foreach (var ping in Pings)
        {
            AveragePing += ping;
        }

        if (AveragePing != 0) AveragePing = (ushort)(AveragePing / Pings.Count);
    }

    // Update the status icon
    public void UpdateStatusIcon()
    {
        if (AveragePing > 0 && AveragePing <= 30)
            StatusIcon = "#31FF7D";
        else if (AveragePing > 30 && AveragePing <= 100)
            StatusIcon = "#FFAA00";
        else
            StatusIcon = "#FF264B";
    }
}