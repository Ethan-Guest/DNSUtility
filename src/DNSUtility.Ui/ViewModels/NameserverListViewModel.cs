using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
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


        RunDnsTest = ReactiveCommand.CreateFromTask(async
            () =>
        {
            var threadPool = new List<Task>();

            foreach (var nameserver in Nameservers.Where(n => n.Country == "US"))
                threadPool.Add(Task.Run(() =>
                {
                    var ping = pingBenchmark.RunBenchmark(nameserver);
                    nameserver.TotalPing = ping;
                }));
            await Task.WhenAll(threadPool);
        });
    }

    public ObservableCollection<Nameserver> Nameservers
    {
        get => _nameservers;
        private set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }
    //public ObservableCollection<Nameserver> Nameservers { get; set; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    /*public ObservableCollection<Nameserver> Nameservers
    {
        get => _nameservers;
        set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }*/
}