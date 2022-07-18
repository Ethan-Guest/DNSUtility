using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DNSUtility.Domain.UserModels;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class SettingsPanelViewModel : ViewModelBase
{
    private readonly UserSettings _userSettings;
    private string _activeInterfaceDescription;

    public SettingsPanelViewModel(UserSettings userSettings, List<string> adapters)
    {
        _userSettings = userSettings;
        _activeInterfaceDescription = userSettings.NetworkAdapters.ActiveInterface.Description;
        Adapters = new List<string>();
        foreach (var adapter in _userSettings.NetworkAdapters.NetworkInterfaces) Adapters.Add(adapter.Description);

        // When the selected nameserver is changed, update the plot
        this.WhenAnyValue(x => x.ActiveInterfaceDescription)
            .ObserveOn(RxApp.MainThreadScheduler)
            .InvokeCommand(UpdateActiveInterface);

        UpdateActiveInterface = ReactiveCommand.Create(
            () =>
            {
                _userSettings.NetworkAdapters.ActiveInterface =
                    _userSettings.NetworkAdapters.NetworkInterfaces.First(i =>
                        i.Description == ActiveInterfaceDescription);
            });
    }

    public ReactiveCommand<Unit, Unit> UpdateActiveInterface { get; set; }

    public string ActiveInterfaceDescription
    {
        get => _activeInterfaceDescription;
        set => this.RaiseAndSetIfChanged(ref _activeInterfaceDescription, value);
    }

    public List<string> Adapters { get; set; }
}