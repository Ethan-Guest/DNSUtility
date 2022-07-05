using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices;

public class LoadNetworkInterfaces : INetworkInterfaces
{
    public NetworkInterface[] RetrieveAllNetworkInterfaces()
    {
        return NetworkInterface.GetAllNetworkInterfaces();
    }
}