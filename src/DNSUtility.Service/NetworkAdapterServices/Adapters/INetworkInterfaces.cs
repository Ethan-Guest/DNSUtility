using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.Adapters;

public interface INetworkInterfaces
{
    NetworkInterface[] GetAllNetworkInterfaces();
    
    NetworkInterface? GetActiveNetworkInterface(NetworkInterface[] adapters);

}