using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public class LoadNetworkInterface : INetworkInterface
{
    public NetworkInterface[] RetriveNetworkInterface()
    {
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        return networkInterfaces;
    }
}