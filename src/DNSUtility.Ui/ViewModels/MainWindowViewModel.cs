using System.Linq;
using DNSUtility.Domain;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices.Adapters;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private GraphViewModel _scatterPlotViewModel;

    public MainWindowViewModel(IParser parser)
    {
        // Create the nameserver list viewmodel
        NameserverListViewModel = new NameserverListViewModel(
            parser.Parse("https://public-dns.info/nameservers.csv").ToList(),
            new PingBenchmark(), this);

        GraphViewModel = new GraphViewModel();
        
        // Create the user configuration
        InitializeNetworkAdapters();
    }

    public NetworkAdapters NetworkAdapters { get; set; }
    public ViewModelBase NameserverListViewModel { get; }

    public GraphViewModel GraphViewModel { get; set; }

    void InitializeNetworkAdapters()
    {
        // Create the interface for retrieving all network adapters
        var networkInterfaces = new LoadNetworkInterfaces();

        // Create the network adapters class
        NetworkAdapters = new NetworkAdapters();
        
        // Store the list of network adapters
        NetworkAdapters.NetworkInterfaces = networkInterfaces.GetAllNetworkInterfaces();

        // Set the default active interface. This can be changed later via a dropdown in the settings UI
        NetworkAdapters.ActiveInterface = networkInterfaces.GetActiveNetworkInterface(NetworkAdapters.NetworkInterfaces);
    }
}