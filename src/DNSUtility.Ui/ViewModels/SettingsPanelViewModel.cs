using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using DNSUtility.Domain.AppModels;
using DNSUtility.Domain.UserModels;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class SettingsPanelViewModel : ViewModelBase
{
    private readonly UserSettings _userSettings;
    private string _activeInterfaceDescription;

    public SettingsPanelViewModel(MainWindowViewModel mainViewModel, UserSettings userSettings, List<string> adapters)
    {
        MainViewModel = mainViewModel;
        _userSettings = userSettings;
        CurrentCountry = userSettings.Country;
        _activeInterfaceDescription = _userSettings.NetworkAdapters.ActiveInterface.Description;
        Adapters = new List<string>();
        foreach (var adapter in _userSettings.NetworkAdapters.NetworkInterfaces) Adapters.Add(adapter.Description);

        CountryCodesList = Enum.GetNames(typeof(CountryInfo.CountryCodes)).ToList();

        UpdateCountryCommand = ReactiveCommand.Create(UpdateCountry);

        UpdateActiveNetworkInterfaceCommand = ReactiveCommand.Create(UpdateActiveNetworkInterface);
    }

    public ReactiveCommand<Unit, Unit> UpdateActiveNetworkInterfaceCommand { get; set; }

    public ReactiveCommand<Unit, Unit> UpdateCountryCommand { get; set; }

    private MainWindowViewModel MainViewModel { get; }
    public List<string> CountryCodesList { get; set; }

    public string CurrentCountry { get; set; }

    public string ActiveInterfaceDescription { get; set; }

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