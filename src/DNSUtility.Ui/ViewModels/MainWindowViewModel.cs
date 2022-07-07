using System.Linq;
using DNSUtility.Domain;
using DNSUtility.Domain.AppModels;
using DNSUtility.Domain.UserSettings;
using DNSUtility.Service.AutoUserConfiguration;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices.Adapters;
using DNSUtility.Service.Parsers;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private GraphViewModel _graph;

    public MainWindowViewModel(IParser parser)
    {
        // Initialize the users configuration
        InitializeUserInfo();
        InitializeNetworkAdapters();

        // Create the nameserver list viewmodel
        NameserverListViewModel = new NameserverListViewModel(
            parser.Parse("https://public-dns.info/nameservers.csv", UserSettings.Country).ToList(),
            new PingBenchmark(), this);
    }



    // The users settings
    public UserSettings UserSettings { get; set; }
    
    // The users network adapters
    public NetworkAdapters NetworkAdapters { get; set; }
    
    // The nameserver list view model
    public ViewModelBase NameserverListViewModel { get; }
    
    // The ping graph view model
    public GraphViewModel GraphViewModel
    {
        get => _graph;
        set => this.RaiseAndSetIfChanged(ref _graph, value);
    }

    // Initialize the users system info. (Country, language) TODO: Add test to check behavior when country / language is null
    private void InitializeUserInfo()
    {
        ICountryInfo countryInfo = new UserCountryCode();
        
        UserSettings = new UserSettings(countryInfo.GetCountryCode());
    }
    
    // Initialize the users network adapters
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