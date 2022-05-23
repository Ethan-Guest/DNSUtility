using System.Linq;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase content;

    public MainWindowViewModel(IParser parser)
    {
        Content = List = new NameserverListViewModel(parser.Parse("https://public-dns.info/nameservers.csv").ToList());
    }

    public ViewModelBase Content
    {
        get => content;
        private set => this.RaiseAndSetIfChanged(ref content, value);
    }

    public NameserverListViewModel List { get; }
}