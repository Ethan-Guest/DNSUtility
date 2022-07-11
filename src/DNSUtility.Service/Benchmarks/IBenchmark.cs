using System.Net.NetworkInformation;

namespace DNSUtility.Service.Benchmarks;

public interface IBenchmark
{
    PingReply Run(string address);
}