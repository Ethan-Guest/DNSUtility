using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface INetworkInterface
{
    NetworkInterface[] RetriveNetworkInterface();
}