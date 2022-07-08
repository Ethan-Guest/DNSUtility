using System.Diagnostics;
using System.Net.NetworkInformation;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public class ApplyDns : IApplyDns
{
    public void ApplyPrimary(Nameserver nameserver, NetworkInterface adapter)
    {
        // TODO switch "ethernet" to adapter name
        var args = $"interface ip set dns \"ethernet\" static {nameserver.IpAddress}";
        RunNetshProcess(args);
    }

    public void ApplySecondary(Nameserver nameserver, NetworkInterface adapter)
    {
        // TODO
    }

    public void ResetPrimary(Nameserver nameserver, NetworkInterface adapter)
    {
        // TODO
    }

    public void ResetSecondary(Nameserver nameserver, NetworkInterface adapter)
    {
        // TODO
    }

    public void ResetAll(Nameserver nameserver, NetworkInterface adapter)
    {
        // TODO
    }

    private void RunNetshProcess(string arguments) // TODO Move to bottom
    {
        Process.Start("netsh.exe", arguments);
    }
}