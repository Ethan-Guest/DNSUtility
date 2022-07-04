using Avalonia.Controls;
using ReactiveUI;
using ScottPlot.Avalonia;

namespace DNSUtility.Ui.ViewModels;

public class ScattPlotViewModel : ViewModelBase
{
    private AvaPlot _scatterPlot;
    
    public ScattPlotViewModel(NameserverViewModel nameserver)
    {
        ScatterPlot = new AvaPlot();

        if (nameserver.Pings.Count != 0)
        {
            double[] dataX = new double[nameserver.Pings.Count];
            double[] dataY = new double[nameserver.Pings.Count];
        
            for (int i = 0; i < nameserver.Pings.Count; i++)
            {
                dataX[i] = i + 1;
                dataY[i] = nameserver.Pings[i];
            }

            //ScatterPlot = ScatterPlot.Find<AvaPlot>("ScattPlot");
            ScatterPlot.Plot.AddScatter(dataX, dataY);
            ScatterPlot.Refresh();
        }
    }
    
    public AvaPlot ScatterPlot
    {
        get => _scatterPlot;
        set => this.RaiseAndSetIfChanged(ref _scatterPlot, value);    
    }


}