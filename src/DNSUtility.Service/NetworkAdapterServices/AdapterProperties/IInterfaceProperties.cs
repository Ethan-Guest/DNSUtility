using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface IInterfaceProperties
{
    IPInterfaceProperties GetInterfaceProperties(NetworkInterface adapter);
}