using Avalonia.Controls;
using ReactiveUI;
using ScottPlot.Avalonia;

namespace DNSUtility.Ui.ViewModels;

public class ScattPlotViewModel : ViewModelBase
{
    private AvaPlot _scatterPlot;

    public ScattPlotViewModel()
    {
        ScatterPlot = new AvaPlot();
        
        double[] dataX = new double[] { 1, 2, 3, 4, 5 };
        double[] dataY = new double[] { 1, 4, 9, 16, 25 };
        
        ScatterPlot.Find<AvaPlot>("ScattPlot");
        ScatterPlot.Plot.AddScatter(dataX, dataY);
        ScatterPlot.Refresh();
    }
    public ScattPlotViewModel(NameserverViewModel nameserver)
    {
        //DefaultPlot.Plot.AddScatter()
        double[] dataX = new double[5];
        double[] dataY = new double[5];

        for (int i = 0; i < 5; i++)
        {
            dataX[i] = i;
            dataY[i] = nameserver.Pings[i];
        }
        ScatterPlot = new AvaPlot();
        ScatterPlot.Find<AvaPlot>("ScattPlot");
        ScatterPlot.Plot.AddScatter(dataX, dataY);
        ScatterPlot.Refresh();
    }
    public AvaPlot ScatterPlot
    {
        get => _scatterPlot;
        set => this.RaiseAndSetIfChanged(ref _scatterPlot, value);    }

}