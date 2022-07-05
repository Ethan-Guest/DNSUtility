using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface INetworkInterfaces
{
    NetworkInterface[] GetAllNetworkInterfaces();
    
    NetworkInterface? GetActiveNetworkInterface(NetworkInterface[] adapters);

}