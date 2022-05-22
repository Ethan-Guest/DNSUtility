using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DNSUtility.Service.Parsers;
using DNSUtility.Ui.ViewModels;
using DNSUtility.Ui.Views;

namespace DNSUtility.Ui;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var parser = new NameserverCsvParser();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(parser)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}