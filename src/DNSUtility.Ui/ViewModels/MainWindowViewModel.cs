using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using DNSUtility.Domain.UserModels;
using DNSUtility.Service.AutoUserConfiguration;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices.Adapters;
using DNSUtility.Service.Parsers;

namespace DNSUtility.Ui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // Private backing fields
    private NameserverListViewModel _nameserverListViewModel;

    public MainWindowViewModel(IParser parser)
    {
        // Initialize the parser object
        NameserverParser = parser;

        // Initialize the users configuration
        InitializeUserSettings();

        // Initialize settings panel
        if (UserSettings != null)
        {
            SettingsPanelViewModel = new SettingsPanelViewModel(this, UserSettings, new List<string>());
        }

        // Create the nameserver list viewmodel
        InitializeNameserverList();

        // Create the live chart view model
        InitializeLiveChart();
    }

    // Properties
    //
    // The parser to be used in nameserver list creation
    public IParser NameserverParser { get; set; }

    // The users settings
    public UserSettings UserSettings { get; set; }

    // The settings panel view model
    public SettingsPanelViewModel SettingsPanelViewModel { get; }

    // The ping graph view model
    public LiveChartViewModel LiveChartViewModel { get; set; }

    // The nameserver list view model
    public NameserverListViewModel NameserverListViewModel { get; set; }


    // Methods
    //
    // Initialize the live chart
    private void InitializeLiveChart()
    {
        LiveChartViewModel = new LiveChartViewModel(this);
    }

    // Initialize the nameserver list
    public void InitializeNameserverList()
    {
        // Parse the public list of nameservers from the users country TODO: Parse a default nameserver if user country is null
        NameserverListViewModel = new NameserverListViewModel(this,
            NameserverParser.Parse("https://public-dns.info/nameservers.csv", UserSettings.Country).ToList(),
            new StandardPingBenchmark(), UserSettings);
    }

    // Initialize the users system info. (Country, language) TODO: Add test to check behavior when country / language is null
    private void InitializeUserSettings()
    {
        // Get country info
        var countryInfo = new UserCountryCode();

        // Load the network interfaces
        var networkInterfaceParser = new LoadNetworkInterfaces();

        // Get all network interfaces
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        // apply the collected information to the user settings class
        UserSettings = new UserSettings(countryInfo.GetCountryCode(), networkInterfaces,
            networkInterfaceParser.GetActiveNetworkInterface(networkInterfaces));
    }
}