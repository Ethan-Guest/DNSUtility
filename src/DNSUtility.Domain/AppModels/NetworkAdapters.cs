using System.Net.NetworkInformation;

namespace DNSUtility.Domain.AppModels;

public class NetworkAdapters
{
    public NetworkAdapters(NetworkInterface[] networkInterfaces, NetworkInterface? activeInterface)
    {
        NetworkInterfaces = networkInterfaces;
        ActiveInterface = activeInterface;
    }

    /// <summary>
    ///     A collection of the systems network interfaces
    /// </summary>
    public NetworkInterface[] NetworkInterfaces { get; set; }

    /// <summary>
    ///     The systems active or default interface
    /// </summary>
    public NetworkInterface? ActiveInterface { get; set; }
}