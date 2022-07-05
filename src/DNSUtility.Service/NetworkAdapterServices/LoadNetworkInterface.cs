using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public class LoadNetworkInterface : INetworkInterface
{
    public NetworkInterface[] RetriveNetworkInterface()
    {
        return NetworkInterface.GetAllNetworkInterfaces();
    }
}