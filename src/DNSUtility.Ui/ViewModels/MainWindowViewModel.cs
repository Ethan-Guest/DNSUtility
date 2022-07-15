using System.Linq;
using System.Net.NetworkInformation;
using DNSUtility.Domain.UserModels;
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
        InitializeUserSettings();

        // Initialize settings panel
        SettingsPanelViewModel = new SettingsPanelViewModel();

        // Create the nameserver list viewmodel
        NameserverListViewModel = new NameserverListViewModel(
            parser.Parse("https://public-dns.info/nameservers.csv", UserSettings.Country).ToList(),
            new StandardPingBenchmark(), this);
    }


    // The users settings
    public UserSettings UserSettings { get; set; }

    // The users network adapters
    /*public NetworkAdapters NetworkAdapters { get; set; }*/

    // The nameserver list view model
    public ViewModelBase NameserverListViewModel { get; }

    // The settings panel view model
    public ViewModelBase SettingsPanelViewModel { get; }

    // The ping graph view model
    public GraphViewModel GraphViewModel
    {
        get => _graph;
        set => this.RaiseAndSetIfChanged(ref _graph, value);
    }

    // Initialize the users system info. (Country, language) TODO: Add test to check behavior when country / language is null
    private void InitializeUserSettings()
    {
        var countryInfo = new UserCountryCode();
        var activeInterface = new LoadNetworkInterfaces();
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        UserSettings = new UserSettings(countryInfo.GetCountryCode(), networkInterfaces,
            activeInterface.GetActiveNetworkInterface(networkInterfaces));
    }

    // Create a new plot from the main view model
    public void CreatePlot(NameserverViewModel? nameserver)
    {
        if (nameserver == null) return;
        GraphViewModel = new GraphViewModel(nameserver);
    }
}