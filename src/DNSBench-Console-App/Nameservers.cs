using System.Net.NetworkInformation;
using DNSUtility.Domain;

namespace DNSBench_Console_App;

public class Nameservers
{
    public const string spaces = "                               ";

    // Initialize list of DNS example servers
    /*public Dictionary<(string providerAddress, string providerName), long> DnsExamples =
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
        };*/


    public int TESTSTORUN = 1;


    public async Task RunPingTest(List<Nameserver> nameservers)
    {
        // list of strings to storing results
        var results = new List<string>();

        /*var threadPool = new List<Task>();*/

        // Loop through 
        foreach (var nameserver in nameservers)
        {
            /*threadPool.Add(Task.Run(() =>
            {*/
            // Create ping sender
            var pingSender = new Ping();

            // Ping the server
            var reply = pingSender.Send(nameserver.IpAddress, 200);

            // Add to the roundtrip time for the current nameserver
            nameserver.TotalPing += reply.RoundtripTime;


            results.Add(
                $"{nameserver.Name}{nameserver.IpAddress} {reply.RoundtripTime}ms");
            /*}));*/
        }

        //await Task.WhenAll(threadPool);

        for (var i = 0; i < results.Count; i++) Console.WriteLine(results[i]);
    }

    // Display the results of the nameserver test
    public void DisplayResults(List<Nameserver> nameservers)
    {
        Console.WriteLine("Host:                Address:                Average Ping:");
        foreach (var nameserver in nameservers)
            Console.WriteLine(
                $"{nameserver.Name}{nameserver.IpAddress} {nameserver.TotalPing / TESTSTORUN}ms");
    }
}