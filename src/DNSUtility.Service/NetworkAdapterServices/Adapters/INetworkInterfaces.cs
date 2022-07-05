using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface INetworkInterfaces
{
    NetworkInterface[] RetrieveAllNetworkInterfaces();
}