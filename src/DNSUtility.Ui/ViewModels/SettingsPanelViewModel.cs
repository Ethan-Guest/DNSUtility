using System.Collections.Generic;
using DNSUtility.Domain.UserModels;

namespace DNSUtility.Ui.ViewModels;

public class SettingsPanelViewModel : ViewModelBase
{
    public SettingsPanelViewModel(UserSettings userSettings, List<string> adapters)
    {
        Adapters = new List<string>();
        foreach (var adapter in userSettings.NetworkAdapters.NetworkInterfaces) Adapters.Add(adapter.Description);
    }

    public List<string> Adapters { get; set; }
}