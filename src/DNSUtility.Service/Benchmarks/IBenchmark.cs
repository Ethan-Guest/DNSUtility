namespace DNSUtility.Service.Benchmarks;

public interface IBenchmark
{
    /// <summary>
    ///     Run a benchmark on an address
    /// </summary>
    /// <param name="address">The address to test</param>
    /// <returns></returns>
    ushort Run(string address);
}