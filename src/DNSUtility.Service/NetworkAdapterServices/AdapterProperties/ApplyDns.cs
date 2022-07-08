using System.Diagnostics;
using System.Net.NetworkInformation;

namespace DNSUtility.Service.NetworkAdapterServices.AdapterProperties;

public class ApplyDns : IApplyDns
{
    // Apply primary: interface ip set dns "interfacename" static x.x.x.x
    // Apply secondary: interface ip ADD dns "interfacename" x.x.x.x index=2
    // Reset both: interface ip set dns "interfacename" DHCP

    // Apply the preferred DNS entry
    public void ApplyPrimary(string ipAddress, NetworkInterface adapter)
    {
        var args =
            $"interface ip set dns \"{adapter.Name}\" static {ipAddress}"; // To apply the preferred dns configuration, set the static ip address
        RunNetshProcess(args);
    }

    // Apply the alternate DNS entry
    public void ApplySecondary(string ipAddress, NetworkInterface adapter)
    {
        var args =
            $"interface ip add dns \"{adapter.Name}\" {ipAddress} index=2"; // To apply the alternate dns, ADD a dns configuration entry
        RunNetshProcess(args);
    }

    public void ResetAll(string ipAddress, NetworkInterface adapter)
    {
        var args =
            $"interface ip set dns \"{adapter.Name}\" DHCP"; // Specifying the DHCP parameter will reset all adapter entries to use dynamic host configuration protocol (default)
        RunNetshProcess(args);
    }

    /// <summary>
    ///     Runs the netsh.exe command tool in windows with provided arguments
    /// </summary>
    /// <param name="args">The command to run in netsh. Expecting apply or reset DNS configuration.</param>
    private void RunNetshProcess(string args)
    {
        Process.Start("netsh.exe", args);
    }
}