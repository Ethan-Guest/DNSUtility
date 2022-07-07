using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public interface IInterfaceProperties
{
    IPInterfaceProperties GetInterfaceProperties(NetworkInterface adapter);
}