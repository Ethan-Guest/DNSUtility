using System.Net.NetworkInformation;
namespace DNSUtility.Domain;

public class NetworkAdapters
{
    public NetworkInterface[] NetworkInterfaces { get; set; }

    public NetworkInterface? ActiveInterface { get; set; }
}