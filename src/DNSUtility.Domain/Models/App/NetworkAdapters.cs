using System.Net.NetworkInformation;

namespace DNSUtility.Domain.AppModels;

/// <summary>
///     A class for managing the network adapters on the system (Windows only)
/// </summary>
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