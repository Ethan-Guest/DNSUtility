using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using DNSUtility.Domain.AppModels;
using DNSUtility.Domain.UserModels;
using DNSUtility.Service.NetworkAdapterServices.AdapterProperties;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class SettingsPanelViewModel : ViewModelBase
{
    private readonly UserSettings _userSettings;
    private string? _selectedNameserver;

    public SettingsPanelViewModel(MainWindowViewModel mainViewModel, UserSettings userSettings, List<string> adapters)
    {
        MainViewModel = mainViewModel;
        _userSettings = userSettings;
        CurrentCountry = userSettings.Country;

        ActiveInterfaceDescription =
            _userSettings.NetworkAdapters.ActiveInterface
                ?.Description; // use LINQ firstordefault to safely check for default / null

        Adapters = new List<string>();
        foreach (var adapter in _userSettings.NetworkAdapters.NetworkInterfaces) Adapters.Add(adapter.Description);

        CountryCodesList = Enum.GetNames(typeof(CountryInfo.CountryCodes)).ToList();

        UpdateCountryCommand = ReactiveCommand.Create(UpdateCountry);

        UpdateActiveNetworkInterfaceCommand = ReactiveCommand.Create(UpdateActiveNetworkInterface);

        // Command for applying nameserver to network adapter
        ApplyDnsCommand = ReactiveCommand.Create<string>(
            parameter =>
            {
                var applyDns = new ApplyDns();

                if (SelectedNameserver != null)
                    // 0 = primary
                    if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)

                        if (parameter == "primary")
                        {
                            if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                                applyDns.ApplyPrimary(SelectedNameserver,
                                    MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
                        }
                        else if (parameter == "secondary")
                        {
                            if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                                applyDns.ApplySecondary(SelectedNameserver,
                                    MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
                        }
            });
        // Command for resetting nameservers in network adapter
        ResetDns = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ResetAll(MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });
    }

    public ReactiveCommand<string, Unit> ApplyDnsCommand { get; set; }

    public ReactiveCommand<Unit, Unit> ResetDns { get; set; }

    public ReactiveCommand<Unit, Unit> UpdateActiveNetworkInterfaceCommand { get; set; }

    public ReactiveCommand<Unit, Unit> UpdateCountryCommand { get; set; }

    private MainWindowViewModel MainViewModel { get; }

    public string? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    public List<string> CountryCodesList { get; set; }

    public string CurrentCountry { get; set; }

    public string? ActiveInterfaceDescription { get; set; }

    public List<string> Adapters { get; set; }

    public void UpdateCountry()
    {
        _userSettings.Country = CurrentCountry;
        MainViewModel.InitializeNameserverList();
    }

    public void UpdateActiveNetworkInterface()
    {
        _userSettings.NetworkAdapters.ActiveInterface =
            _userSettings.NetworkAdapters.NetworkInterfaces.FirstOrDefault(i =>
                i.Description == ActiveInterfaceDescription);
    }
}