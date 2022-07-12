using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public interface IApplyDns
{
    void ApplyPrimary(string ipAddress, NetworkInterface adapter);
    void ApplySecondary(string ipAddress, NetworkInterface adapter);
    void ResetAll(NetworkInterface adapter);
}