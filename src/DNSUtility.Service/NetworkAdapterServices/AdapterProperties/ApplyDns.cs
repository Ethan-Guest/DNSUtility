using System.Net.NetworkInformation;
using DNSUtility.Domain;

namespace DNSUtility.Service.NetworkAdapterServices;

public class ApplyDns : IApplyDns
{
    public void ApplyPreferredDns(Nameserver nameserver, NetworkInterface adapter)
    {
        // Set the dns server
    }

    public void ApplyAlternateDns(Nameserver nameserver, NetworkInterface adapter)
    { 
        // Set the dns server
    }
}