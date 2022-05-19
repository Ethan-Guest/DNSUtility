using System.Net.NetworkInformation;

namespace DNSBench_Console_App;

public class User
{
    // The users primary DNS
    public string primaryDNS { get; set; }

    // The users secondary DNS
    public string secondaryDNS { get; set; }

    // The users local hostname
    public string localHostName { get; set; }

    public void Initialize()
    {
        var adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var adapter in adapters)
            if (adapter.Name == "Ethernet")
            {
                var adapterProperties = adapter.GetIPProperties();
                var dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0) primaryDNS = dnsServers[0].ToString();
                if (dnsServers.Count > 1) secondaryDNS = dnsServers[1].ToString();
            }

        // Initialize the users primary DNS
        //this.primaryDNS = 
        // Initialize the users secondary DNS
    }
}