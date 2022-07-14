using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNSUtility.Domain.AppModels;
using DNSUtility.Service.Benchmarks;
using DNSUtility.Service.NetworkAdapterServices.AdapterProperties;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    private bool _benchmarkInProgress;
    private NameserverViewModel? _selectedNameserver;

    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark,
        MainWindowViewModel mainWindowViewModel)
    {
        // Get access to main view model instance
        MainViewModel = mainWindowViewModel;

        // Initialize the list of nameservers for the UI
        Nameservers = new ObservableCollection<NameserverViewModel>();

        // Convert list of nameservers to a list of NameserverViewModels
        foreach (var nameserver in nameservers)
        {
            var nameserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(nameserverViewModel);
        }

        BenchmarkTasks = new List<Task>();

        RunDnsTest = ReactiveCommand.Create(
            () =>
            {
                // Notify the UI that a benchmark is in progress
                BenchmarkInProgress = true;

                // Use half of the users resources to run the test
                ThreadPool.GetMaxThreads(out var workerThreads, out var completionPortThreads);
                ThreadPool.SetMinThreads(workerThreads / 2, completionPortThreads / 2);

                // Create a task for each nameserver test
                foreach (var nameserver in Nameservers)
                    BenchmarkTasks.Add(BenchmarkNameserver(nameserver, pingBenchmark));

                // Wait until all the tasks have completed
                Task.WaitAll(BenchmarkTasks.ToArray());
                RemovePoorQualityNameservers();

                BenchmarkInProgress = false;
            });

        // TODO: Merge the following TWO commands into ONE function with different parameters 
        // Command for applying primary dns server
        ApplyPrimary = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ApplyPrimary(SelectedNameserver.IpAddress,
                            MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });
        // Command for applying secondary dns server
        ApplySecondary = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ApplySecondary(SelectedNameserver.IpAddress,
                            MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });
        ResetDns = ReactiveCommand.Create(
            () =>
            {
                var applyDns = new ApplyDns();
                if (MainViewModel.UserSettings.NetworkAdapters.ActiveInterface != null)
                    if (SelectedNameserver != null)
                        applyDns.ResetAll(MainViewModel.UserSettings.NetworkAdapters.ActiveInterface);
            });

        this.WhenAnyValue(x => x.SelectedNameserver)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(CreatePlot);
    }

    public List<Task> BenchmarkTasks { get; set; }

    public bool BenchmarkInProgress
    {
        get => _benchmarkInProgress;
        set => this.RaiseAndSetIfChanged(ref _benchmarkInProgress, value);
    }

    public ReactiveCommand<Unit, Unit> ResetDns { get; set; }

    public ObservableCollection<NameserverViewModel> Nameservers { get; }

    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    public ReactiveCommand<Unit, Unit> ApplyPrimary { get; set; }

    public ReactiveCommand<Unit, Unit> ApplySecondary { get; set; }

    public NameserverViewModel? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    public MainWindowViewModel MainViewModel { get; set; }

    private Task BenchmarkNameserver(NameserverViewModel nameserver, IBenchmark pingBenchmark)
    {
        var t = Task.Run(() =>
        {
            Task.Delay(10000);

            for (var j = 0; j < 5; j++)
            {
                var ping = pingBenchmark.Run(nameserver.IpAddress);

                // Set the reply as the latest ping
                nameserver.LatestPing = ping;

                // Add the reply to the list of pings
                nameserver.Pings.Add(ping);

                // Calculate the average ping so it can be displayed in the view
                nameserver.CalculateAveragePing();
            }
        });
        return Task.FromResult(t);
    }

    /// <summary>
    ///     A method to remove the poor quality nameservers.
    /// </summary>
    private void RemovePoorQualityNameservers()
    {
        for (var i = 0; i < Nameservers.Count; i++)
            if (Nameservers[i].LatestPing == 0)
                Nameservers.RemoveAt(i);
    }

    private async void CreatePlot(NameserverViewModel? nameserver)
    {
        if (nameserver == null)
        {
            return;
        }

        MainViewModel.GraphViewModel = new GraphViewModel(nameserver);
    }
}