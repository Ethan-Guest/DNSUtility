using DNSUtility.Domain;

namespace DNSUtility.Service.Benchmarks;

public interface IBenchmark
{
    ushort RunBenchmark(string address);
}