using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DNSUtility.Ui.Views;

public partial class GraphView : UserControl
{
    public GraphView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}