using System.Linq;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _nameserverListContent;

    public MainWindowViewModel(IParser parser)
    {
        NameserverListViewModel = new NameserverListViewModel(parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark());
        NameserverListContent = NameserverListViewModel;
    }

    public ViewModelBase NameserverListContent
    {
        get => _nameserverListContent;
        private set => this.RaiseAndSetIfChanged(ref _nameserverListContent, value);
    }

    public NameserverListViewModel NameserverListViewModel { get; }
}