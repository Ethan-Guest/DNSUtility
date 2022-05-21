﻿using DNSUtility.Domain;

namespace DNSUtility.Service.Parsers;

/// <summary>
///     An interface for parsing different files
/// </summary>
public interface IParser
{
    IEnumerable<Nameserver> Parse(string path);
}