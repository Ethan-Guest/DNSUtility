using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IParser parser)
    {
        NameserverListViewModel = new NameserverListViewModel(parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark());
        NameserverListContent = NameserverListViewModel;
    }

    public ViewModelBase NameserverListContent { get; set; }

    public NameserverListViewModel NameserverListViewModel { get; }
}