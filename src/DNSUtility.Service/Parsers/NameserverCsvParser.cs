﻿using System.Globalization;
using System.Net;
using CsvHelper;
using DNSUtility.Domain;

namespace DNSUtility.Service.Parsers;

public class NameserverCsvParser : IParser
{
    public IEnumerable<Nameserver> Parse(string path)
    {
        var client = new WebClient();
        var stream = client.OpenRead(path);
        using var reader = new StreamReader(stream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Nameserver>().ToList();
    }
}