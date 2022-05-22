using System.Collections.Generic;
using System.Collections.ObjectModel;
using DNSUtility.Domain;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    public NameserverListViewModel(IEnumerable<Nameserver> nameservers)
    {
        Nameservers = new ObservableCollection<Nameserver>(nameservers);
    }

    public ObservableCollection<Nameserver> Nameservers { get; set; }
}