using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

/// <summary>
///     Apply and reset primary and secondary DNS servers to the system (Windows only)
/// </summary>
public interface IApplyDns
{
    void ApplyPrimary(string ipAddress, NetworkInterface adapter);
    void ApplySecondary(string ipAddress, NetworkInterface adapter);
    void ResetAll(NetworkInterface adapter);
}