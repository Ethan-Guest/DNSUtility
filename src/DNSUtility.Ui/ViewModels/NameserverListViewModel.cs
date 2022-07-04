using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Avalonia.Collections;
using Avalonia.Controls;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using DynamicData.Binding;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark)
    {
        Nameservers = new ObservableCollection<NameserverViewModel>();

        // Convert list of nameservers to a list of NameserverViewModels
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
                        // Run the test 5 times
                        for (int i = 0; i < 5; i++)
                        {
                            ushort ping = pingBenchmark.RunBenchmark(nameserver.IpAddress);
                            

                            // Set the reply as the latest ping
                            nameserver.LatestPing = ping;
                        
                            // Add the reply to the list of pings
                            nameserver.Pings.Add(ping);
                            
                            // If the ping replies with 0, remove it from the list
                            /*if (ping == 0)
                            {
                                Nameservers.Remove(nameserver);
                                break;
                            }*/
                        }
                    }).Start(); 
            });
    }
    
    public ObservableCollection<NameserverViewModel> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }
    
}