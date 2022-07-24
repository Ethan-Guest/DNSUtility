using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.Adapters;

public class LoadNetworkInterfaces : INetworkInterfaces
{
    /// <summary>
    ///     Get the active network interface from the system
    /// </summary>
    /// <param name="adapters">A collection of all adapters on the system</param>
    /// <returns>The active adapter</returns>
    public NetworkInterface? GetActiveNetworkInterface(NetworkInterface[] adapters)
    {
        // Return the first adapter where the operational status is UP and has a Ethernet or Wifi interface
        return adapters.FirstOrDefault(a =>
            a.OperationalStatus == OperationalStatus.Up &&
            (a.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
             a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211));
    }
}