using System.Linq;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.Parsers;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IParser parser)
    {
        NameserverListViewModel = new NameserverListViewModel(
            parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark());
        NameserverListContent = NameserverListViewModel;

        ScattPlot = new ScattPlotViewModel();
    }

    public ViewModelBase NameserverListContent { get; set; }

    public ViewModelBase NameserverListViewModel { get; }
    
    public ViewModelBase ScattPlot { get; }

}