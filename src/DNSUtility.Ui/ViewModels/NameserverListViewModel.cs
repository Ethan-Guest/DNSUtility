using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark)
    {
        Nameservers = new ObservableCollection<NameserverViewModel>();

        int count = 0;
        
        foreach (var nameserver in nameservers)
        {
            var vm = new NameserverViewModel(nameserver);
            Nameservers.Add(vm);
            count++;
            if (count == 10)
            {
                break;
            }
        }

        RunDnsTest = ReactiveCommand.Create(
            () =>
            {

                foreach (var nameserver in Nameservers)
                    // Create a new thread for each test
                    new Thread(() =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            var ping = pingBenchmark.RunBenchmark(nameserver.IpAddress);
                            nameserver.TotalPing = ping;
                        }
                        // Run the test
                    }).Start();
            });
    }
    
    public ObservableCollection<NameserverViewModel> Nameservers { get; }
    //public ObservableCollection<Nameserver> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    /*public ObservableCollection<Nameserver> Nameservers
    {
        get => _nameservers;
        set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }*/
}