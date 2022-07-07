﻿using System.Net.NetworkInformation;
using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;

namespace DNSUtility.Service.NetworkAdapterServices;

public interface IApplyDns
{
    void ApplyPreferredDns(string ipAddress, NetworkInterface adapter);
    
    void ApplyAlternateDns(Nameserver nameserver, NetworkInterface adapter);
}