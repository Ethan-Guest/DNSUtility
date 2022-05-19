using System.Net.NetworkInformation;

namespace DNSBench_Console_App;

public class Nameservers
{
    public const string spaces = "                               ";

    // Initialize list of DNS example servers
    public Dictionary<(string providerAddress, string providerName), long> DnsExamples =
        new()
        {
            {("156.154.70.22", "US - Comodo"), 0},
            {("156.154.71.22", "US - Comodo"), 0},
            {("8.8.4.4", "US - Google Public DNS"), 0},
            {("8.8.8.8", "US - Google Public DNS"), 0},
            {("204.74.101.1", "US - UltraDNS"), 0},
            {("204.69.234.1", "US - UltraDNS"), 0},
            {("208.67.222.222", "US - OpenDNS"), 0},
            {("208.67.220.220", "US - OpenDNS"), 0}
        };

    public int TESTSTORUN = 10;


    public void RunPingTest()
    {
        // list of strings to storing results
        var results = new List<string>();


        // Loop through 
        foreach (var nameserver in DnsExamples)
        {
            // Create ping sender
            var pingSender = new Ping();

            // Ping the server
            var reply = pingSender.Send(nameserver.Key.providerAddress);

            // Add to the roundtrip time for the current nameserver
            DnsExamples[nameserver.Key] += reply.RoundtripTime;


            results.Add(
                $"{nameserver.Key.providerName}{spaces.Remove(0, nameserver.Key.providerName.Length)}{nameserver.Key.providerAddress}{spaces.Remove(0, nameserver.Key.providerAddress.Length)}{reply.RoundtripTime}ms");
        }

        for (var i = 0; i < results.Count; i++) Console.WriteLine(results[i]);
    }

    // Display the results of the nameserver test
    public void DisplayResults()
    {
        Console.WriteLine($"Host:{spaces.Remove(0, 6)} Address: {spaces.Remove(0, 9)}Ping:");
        foreach (var dnsAddress in DnsExamples)
            Console.WriteLine(
                $"{dnsAddress.Key.providerName}{spaces.Remove(0, dnsAddress.Key.providerName.Length)}{dnsAddress.Key.providerAddress}{spaces.Remove(0, dnsAddress.Key.providerAddress.Length)}{dnsAddress.Value / TESTSTORUN}ms");
    }
}