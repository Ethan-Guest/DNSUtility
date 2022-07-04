using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using Avalonia.Collections;
using Avalonia.Controls;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark)
    {
        Nameservers = new ObservableCollection<NameserverViewModel>();

        //int count = 0;
        
        foreach (var nameserver in nameservers)
        {
            var namserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(namserverViewModel);
        }

        RunDnsTest = ReactiveCommand.Create(
            () =>
            {
                foreach (var nameserver in Nameservers)
                    // Create a new thread for each test
                    new Thread(() =>
                    {
                        // Run the test
                        var ping = pingBenchmark.RunBenchmark(nameserver.IpAddress);
                        
                        // Set the reply as the latest ping
                        nameserver.LatestPing = ping;
                        
                        // Add the reply to the list of pings
                        nameserver.Pings.Add(ping);
                    }).Start();
            });
    }
    
    public ObservableCollection<NameserverViewModel> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }
    
}