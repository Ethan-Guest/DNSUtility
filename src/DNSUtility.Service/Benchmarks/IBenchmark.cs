namespace DNSUtility.Service.Benchmarks;

public interface IBenchmark
{
    ushort Run(string address);
}