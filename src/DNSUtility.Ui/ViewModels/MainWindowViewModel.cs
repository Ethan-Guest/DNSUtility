using System.Linq;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ScattPlotViewModel _scatterPlotViewModel;

    public MainWindowViewModel(IParser parser)
    {
        // Create the nameserver list viewmodel
        NameserverListViewModel = new NameserverListViewModel(
            parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark(), this);
        
        // Create the user configuration
        InitializeNetworkAdapters();
    }

    public NetworkAdapters NetworkAdapters { get; set; }
    public ViewModelBase NameserverListViewModel { get; }

    public ScattPlotViewModel? ScatterPlotViewModel
    {
        get => _scatterPlotViewModel;
        set => this.RaiseAndSetIfChanged(ref _scatterPlotViewModel, value);
    }

    void InitializeNetworkAdapters()
    {
        NetworkAdapters = new NetworkAdapters();
        INetworkInterfaces networkInterfaces = new LoadNetworkInterfaces();
        NetworkAdapters.NetworkInterfaces = networkInterfaces.RetrieveAllNetworkInterfaces();
    }
}