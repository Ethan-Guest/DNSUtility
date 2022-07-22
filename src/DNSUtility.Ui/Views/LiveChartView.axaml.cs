using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DNSUtility.Ui.Views;

public partial class LiveChartView : UserControl
{
    public LiveChartView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}