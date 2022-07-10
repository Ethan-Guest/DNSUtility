using System.Net.NetworkInformation;
using System.Text;

namespace DNSUtility.Service.Benchmarks;

public class StandardPingBenchmark : IBenchmark
{
    public ushort Run(string address)
    {
        // Create ping sender
        var pingSender = new Ping();

        // Generate data to send
        var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var buffer = Encoding.ASCII.GetBytes(data);

        // Ping the server
        var reply = pingSender.Send(address, 10000, buffer);

        return (ushort)reply.RoundtripTime;
    }
}