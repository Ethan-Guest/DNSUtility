using DNSUtility.Domain;

namespace DNSUtility.Service.Benchmarks;

public interface IBenchmark
{
    long RunBenchmark(string address);
}