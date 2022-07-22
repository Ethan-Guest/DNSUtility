using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

namespace DNSUtility.Ui.ViewModels;

public class LiveChartViewModel : ViewModelBase
{
    private readonly ObservableCollection<ObservableValue> _observableValues;

    public LiveChartViewModel()
    {
        _observableValues = new ObservableCollection<ObservableValue>();

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>()
        };
    }

    public ObservableCollection<ISeries> Series { get; set; }
}