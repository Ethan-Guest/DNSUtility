﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    private ObservableCollection<Nameserver> _nameservers;


    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark)
    {
        Nameservers = new ObservableCollection<Nameserver>(nameservers);


        RunDnsTest = ReactiveCommand.Create(
            () =>
            {
                foreach (var nameserver in Nameservers.Where(n => n.Country == "US"))
                {
                    var ping = pingBenchmark.RunBenchmark(nameserver);
                    nameserver.TotalPing = ping;
                }
            });
    }

    public ObservableCollection<Nameserver> Nameservers { get; set; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    /*public ObservableCollection<Nameserver> Nameservers
    {
        get => _nameservers;
        set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }*/
}