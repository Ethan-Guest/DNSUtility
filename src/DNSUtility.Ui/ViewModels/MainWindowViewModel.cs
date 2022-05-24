using System.Linq;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase content;

    public MainWindowViewModel(IParser parser)
    {
        Content = List = new NameserverListViewModel(parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark());
    }

    public ViewModelBase Content
    {
        get => content;
        private set => this.RaiseAndSetIfChanged(ref content, value);
    }

    public NameserverListViewModel List { get; }
}