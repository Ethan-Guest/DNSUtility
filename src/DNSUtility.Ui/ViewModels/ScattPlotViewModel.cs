using System.Drawing;
using System.Drawing.Drawing2D;
using ReactiveUI;
using ScottPlot;
using ScottPlot.Avalonia;

namespace DNSUtility.Ui.ViewModels;

public class ScattPlotViewModel : ViewModelBase
{
    private AvaPlot _scatterPlot;

    public ScattPlotViewModel(NameserverViewModel nameserver)
    {
        ScatterPlot = new AvaPlot();

        // Set the plot style
        // Disable zooming
        /*ScatterPlot.Configuration.Zoom = false;

        // Disable panning
        ScatterPlot.Configuration.Pan = false;*/

        // Disable double click benchmark stats
        ScatterPlot.Configuration.DoubleClickBenchmark = false;
        
        // Disable frame
        ScatterPlot.Plot.Frameless();
        
        // Set axis limits
        /*ScatterPlot.Plot.SetAxisLimits(yMin: 0);*/

        
        ScatterPlot.Plot.Style(Color.Transparent,
            Color.FromArgb(1, 39, 39, 60),
            Color.FromArgb(53, 53, 83),
            Color.FromArgb(53, 53, 83));
        

        if (nameserver.Pings.Count != 0)
        {
            var dataX = new double[nameserver.Pings.Count];
            var dataY = new double[nameserver.Pings.Count];

            for (var i = 0; i < nameserver.Pings.Count; i++)
            {
                dataX[i] = i + 1;
                dataY[i] = nameserver.Pings[i];
            }
            
            var scatterPlotData = ScatterPlot.Plot.AddSignal(dataY,  1, Color.FromArgb(49, 255, 125));
            
            
            scatterPlotData.FillBelow(Color.FromArgb(49, 255, 125), Color.FromArgb(125,49, 255, 125));
            
   
            //scatterPlotData.Smooth = true;

            /*ScatterPlot.Plot.AddFill(dataX, dataY, color: Color.FromArgb(50, 49, 255, 125));
*/
            ScatterPlot.Refresh();
        }
    }

    public AvaPlot ScatterPlot
    {
        get => _scatterPlot;
        set => this.RaiseAndSetIfChanged(ref _scatterPlot, value);
    }
}