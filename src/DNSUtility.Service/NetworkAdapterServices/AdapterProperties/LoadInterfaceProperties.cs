using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public class LoadInterfaceProperties : IInterfaceProperties
{
    public IPInterfaceProperties GetInterfaceProperties(NetworkInterface adapter)
    {
        return adapter.GetIPProperties();
    }
}