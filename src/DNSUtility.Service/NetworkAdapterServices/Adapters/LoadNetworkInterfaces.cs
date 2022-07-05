using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.Adapters;

public class LoadNetworkInterfaces : INetworkInterfaces
{
    public NetworkInterface[] GetAllNetworkInterfaces()
    {
        return NetworkInterface.GetAllNetworkInterfaces();
    }

    public NetworkInterface? GetActiveNetworkInterface(NetworkInterface?[] adapters)
    {
        return adapters.FirstOrDefault(a =>
            a != null && (a.OperationalStatus == OperationalStatus.Up &&
                          (a.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                          a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)));
    }
}