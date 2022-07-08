using System.Net.NetworkInformation;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public interface IApplyDns
{
    void ApplyPrimary(Nameserver nameserver, NetworkInterface adapter);
    void ApplySecondary(Nameserver nameserver, NetworkInterface adapter);
    void ResetPrimary(Nameserver nameserver, NetworkInterface adapter);
    void ResetSecondary(Nameserver nameserver, NetworkInterface adapter);
    void ResetAll(Nameserver nameserver, NetworkInterface adapter);
}