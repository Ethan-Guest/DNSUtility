using System.Net.NetworkInformation;
using DNSUtility.Domain;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface IApplyDns
{
    void ApplyPreferredDns(Nameserver nameserver, NetworkInterface adapter);
    
    void ApplyAlternateDns(Nameserver nameserver, NetworkInterface adapter);
}