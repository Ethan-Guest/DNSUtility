using System.Net.NetworkInformation;

namespace DNSUtility.Service.Benchmarks;

public class QuickPingBenchmark : IBenchmark
{
    public ushort Run(string address)
    {
        // Create ping sender
        var pingSender = new Ping();

        // Ping the server
        var reply = pingSender.Send(address, 10000);

        return (ushort)reply.RoundtripTime;
    }
}