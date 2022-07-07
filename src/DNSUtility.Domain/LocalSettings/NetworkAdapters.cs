using System.Net.NetworkInformation;

namespace DNSUtility.Domain.LocalSettings;

public class NetworkAdapters
{
    public NetworkInterface[] NetworkInterfaces { get; set; }

    public NetworkInterface? ActiveInterface { get; set; }
}