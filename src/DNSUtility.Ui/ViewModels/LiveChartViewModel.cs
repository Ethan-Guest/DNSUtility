using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;

namespace DNSUtility.Ui.ViewModels;

public class LiveChartViewModel : ViewModelBase
{
    private readonly ObservableCollection<ObservableValue> _observable;
}