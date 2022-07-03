using System.Net.NetworkInformation;
using DNSUtility.Domain;

namespace DNSUtility.Service.Benchmarks;

public class PingBenchmark : IBenchmark
{
    public ushort RunBenchmark(string address)
    {
        // Create ping sender
        var pingSender = new Ping();

        // Ping the server
        var reply = pingSender.Send(address, 10000);

        return (ushort)reply.RoundtripTime;
    }
}