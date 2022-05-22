using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DNSUtility.Ui.Views;

public partial class NameserverListView : UserControl
{
    public NameserverListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}