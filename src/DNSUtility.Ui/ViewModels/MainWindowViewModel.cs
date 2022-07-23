using System.Collections.Generic;
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
    private ViewModelBase _nameserverListViewModel;

    public MainWindowViewModel(IParser parser)
    {
        // Initialize the parser object
        NameserverParser = parser;

        // Initialize the users configuration
        InitializeUserSettings();

        // TODO handle usersettings null
        // Initialize settings panel
        if (UserSettings != null)
        {
            SettingsPanelViewModel = new SettingsPanelViewModel(this, UserSettings, new List<string>());
        }

        // Create the nameserver list viewmodel
        InitializeNameserverList();

        InitializeLiveChart();
    }

    public IParser NameserverParser { get; set; }

    // The users settings
    public UserSettings UserSettings { get; set; }

    // The users network adapters
    /*public NetworkAdapters NetworkAdapters { get; set; }*/

    // The nameserver list view model
    public ViewModelBase NameserverListViewModel
    {
        get => _nameserverListViewModel;
        set => this.RaiseAndSetIfChanged(ref _nameserverListViewModel, value);
    }

    // The settings panel view model
    public SettingsPanelViewModel SettingsPanelViewModel { get; }

    // The ping graph view model
    public LiveChartViewModel LiveChartViewModel { get; set; }

    private void InitializeLiveChart()
    {
        LiveChartViewModel = new LiveChartViewModel(this);
    }

    public void InitializeNameserverList()
    {
        NameserverListViewModel = new NameserverListViewModel(
            NameserverParser.Parse("https://public-dns.info/nameservers.csv", UserSettings.Country).ToList(),
            new StandardPingBenchmark(), this);
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
}