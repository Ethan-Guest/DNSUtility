using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
        // Initialize reference to main view model and settings
        MainViewModel = mainViewModel;
        _userSettings = userSettings;

        // Retrieve the active network adapter description (to be used in the network adapter combo box)
        ActiveInterfaceDescription =
            _userSettings.NetworkAdapters.ActiveInterface
                ?.Description; // TODO use LINQ firstordefault to safely check for default / null

        // A list of adapter descriptions
        AdapterDescriptions = new List<string>();
        foreach (var adapter in _userSettings.NetworkAdapters.NetworkInterfaces)
            AdapterDescriptions.Add(adapter.Description);

        // Initialize the service for applying DNS servers
        ApplyDns = new ApplyDns();

        // A command for updating the active network interface
        UpdateActiveNetworkInterfaceCommand = ReactiveCommand.Create(UpdateActiveNetworkInterface);

        // Command for applying nameserver to network adapter
        ApplyDnsCommand = ReactiveCommand.Create<string>(
            parameter =>
            {
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    // Set the preferred DNS
                    if (parameter == "primary")
                    {
                        if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                            if (PreferredTextInput != null)

                                ApplyDns.ApplyPrimary(PreferredTextInput,
                                    MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
                    }
                    // Set the alternate DNS
                    else if (parameter == "secondary")
                    {
                        if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                            if (AlternateTextInput != null)
                                ApplyDns.ApplySecondary(AlternateTextInput,
                                    MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
                    }
            });

        // Command for resetting nameservers in network adapter
        ResetDnsCommand = ReactiveCommand.Create(
            () =>
            {
                // Create the service for applying DNS
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    ApplyDns.ResetAll(MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });
    }

    // Commands
    //
    // Command for applying a DNS server
    public ReactiveCommand<string, Unit> ApplyDnsCommand { get; set; }

    // Command for resetting a DNS server
    public ReactiveCommand<Unit, Unit> ResetDnsCommand { get; set; }

    // Command for updating the active network interface
    public ReactiveCommand<Unit, Unit> UpdateActiveNetworkInterfaceCommand { get; set; }

    // Properties
    //
    // A reference to the main view model
    private MainWindowViewModel MainViewModel { get; }

    // The service for applying a DNS server
    public ApplyDns ApplyDns { get; set; }

    // The description of the active network interface
    public string? ActiveInterfaceDescription { get; set; }

    // A list of all the network adapters descriptions
    public List<string> AdapterDescriptions { get; set; }

    // Store the input text of the primary nameserver textbox
    public string PreferredTextInput { get; set; }

    // Store the input text of the secondary nameserver textbox
    public string AlternateTextInput { get; set; }

    // Methods
    //
    // Update the active network interface (from the combobox)
    public void UpdateActiveNetworkInterface()
    {
        // Set the active network interface as the first interface that matches the description
        _userSettings.NetworkAdapters.ActiveInterface =
            _userSettings.NetworkAdapters.NetworkInterfaces.FirstOrDefault(i =>
                i.Description == ActiveInterfaceDescription);
    }
}