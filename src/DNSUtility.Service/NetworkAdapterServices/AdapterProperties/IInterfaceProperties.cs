using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public interface IInterfaceProperties
{
    /// <summary>
    ///     Get the properties of an interface
    /// </summary>
    /// <param name="adapter">The adapter to retrieve information from</param>
    /// <returns>Interface properties</returns>
    IPInterfaceProperties GetInterfaceProperties(NetworkInterface adapter);
}