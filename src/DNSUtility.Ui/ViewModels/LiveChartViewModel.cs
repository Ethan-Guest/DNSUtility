using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace DNSUtility.Ui.ViewModels;

public class LiveChartViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ObservableCollection<ObservableValue> _observableValues;

    public LiveChartViewModel(MainWindowViewModel mainViewModel)
    {
        // Reference to main view model
        _mainViewModel = mainViewModel;

        _observableValues = new ObservableCollection<ObservableValue>();

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _observableValues,
                Fill = new LinearGradientPaint(new SKColor(49, 255, 125, 1), new SKColor(49, 255, 125, 125),
                    new SKPoint(0.5f, 1), new SKPoint(0.5f, 0)),
                Stroke = new SolidColorPaint(new SKColor(49, 255, 125), 4),
                GeometryFill = new SolidColorPaint(new SKColor(49, 255, 125)),
                GeometrySize = 10,
                GeometryStroke = null
            }
        };
    }

    // LIVE CHART STYLING

    // The frame style
    public DrawMarginFrame DrawMarginFrame => new()
    {
        Fill = new SolidColorPaint(new SKColor(0, 0, 0, 20)),
        Stroke = new SolidColorPaint(new SKColor(53, 53, 83), 2)
    };

    // X Axis style
    public Axis[] XAxes { get; set; }
        =
        {
            new()
            {
                LabelsPaint = new SolidColorPaint(new SKColor(160, 160, 212)),
                MinLimit = 0,
                TextSize = 15,
                SeparatorsPaint = new SolidColorPaint(new SKColor(53, 53, 83)) { StrokeThickness = 2 }
            }
        };

    // Y Axis style
    public Axis[] YAxes { get; set; }
        =
        {
            new()
            {
                LabelsPaint = new SolidColorPaint(new SKColor(160, 160, 212)),
                TextSize = 15,
                SeparatorsPaint = new SolidColorPaint(new SKColor(53, 53, 83)) { StrokeThickness = 2 }
            }
        };

    // PROPERTIES

    /// <summary>
    ///     An observable collection of series to be displayed on the live chart. Multiple plots can be displayed at once, thus
    ///     we create a collection.
    /// </summary>
    public ObservableCollection<ISeries> Series { get; set; }
}