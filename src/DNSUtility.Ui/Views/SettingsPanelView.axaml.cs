using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DNSUtility.Ui.Views;

public partial class SettingsPanelView : UserControl
{
    public SettingsPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}