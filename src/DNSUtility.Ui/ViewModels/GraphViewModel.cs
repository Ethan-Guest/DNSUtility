using System.Drawing;
using System.Drawing.Drawing2D;
using ReactiveUI;
using ScottPlot;
using ScottPlot.Avalonia;

namespace DNSUtility.Ui.ViewModels;

public class GraphViewModel : ViewModelBase
{
    private AvaPlot _graph;

    public GraphViewModel(NameserverViewModel nameserver)
    {
        Graph = new AvaPlot();

        // Set the plot style
        // Disable zooming
        /*ScatterPlot.Configuration.Zoom = false;

        // Disable panning
        ScatterPlot.Configuration.Pan = false;*/

        // Disable double click benchmark stats
        Graph.Configuration.DoubleClickBenchmark = false;
        
        
        // Disable frame
        //ScatterPlot.Plot.Frameless();
        
        // Set axis limits
        /*ScatterPlot.Plot.SetAxisLimits(yMin: 0);*/

        
        Graph.Plot.Style(Color.Transparent,
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
            
            var graphPlotData = Graph.Plot.AddSignal(dataY,  1, Color.FromArgb(49, 255, 125));
            
            
            graphPlotData.FillBelow(Color.FromArgb(49, 255, 125), Color.FromArgb(125,49, 255, 125));
            
            Graph.Plot.AxisAuto(0); 

            //scatterPlotData.Smooth = true;

            /*ScatterPlot.Plot.AddFill(dataX, dataY, color: Color.FromArgb(50, 49, 255, 125));
*/
            Graph.Refresh();
        }
    }

    public AvaPlot Graph
    {
        get => _graph;
        set => this.RaiseAndSetIfChanged(ref _graph, value);
    }
}