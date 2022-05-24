﻿using System.Net.NetworkInformation;
using DNSUtility.Domain;

namespace DNSUtility.Service.Benchmarks;

public class PingBenchmark : IBenchmark
{
    public long RunBenchmark(Nameserver nameserver)
    {
        // Create ping sender
        var pingSender = new Ping();

        // Ping the server
        var reply = pingSender.Send(nameserver.IpAddress, 500);

        return reply.RoundtripTime;
    }
}