using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.Adapters;

public interface INetworkInterfaces
{
    /// <summary>
    ///     Get the active network interface from the system
    /// </summary>
    /// <param name="adapters">A collection of all adapters on the system</param>
    /// <returns>The active adapter</returns>
    NetworkInterface? GetActiveNetworkInterface(NetworkInterface[] adapters);
}