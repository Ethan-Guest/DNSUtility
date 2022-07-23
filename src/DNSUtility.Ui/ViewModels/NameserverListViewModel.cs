using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNSUtility.Domain.AppModels;
using DNSUtility.Service.Benchmarks;
using LiveChartsCore.Defaults;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    // Backing fields
    private bool _benchmarkInProgress;
    private int _completedTaskCounter;
    private ObservableCollection<NameserverViewModel>? _nameservers;

    private NameserverViewModel? _selectedNameserver;

    // Constructor takes in a list of nameserver models, the benchmark object, and a reference to the main ViewModel
    // TODO switch the ping benchmark to the selected benchmark type from user settings. (Quick, Standard)
    public NameserverListViewModel(IEnumerable<Nameserver> nameservers, IBenchmark pingBenchmark,
        MainWindowViewModel mainWindowViewModel)
    {
        // Store access to main view model instance
        MainViewModel = mainWindowViewModel;

        // Initialize the list of nameservers for the UI
        Nameservers = new ObservableCollection<NameserverViewModel>();
        foreach (var nameserver in nameservers)
        {
            var nameserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(nameserverViewModel);
        }

        // List of tasks to be run during test
        BenchmarkTasks = new List<Task>();

        // Shared lock object for use in test
        Lock = new object();

        // Count the number of completed tasks
        CompletedTaskCounter = 0;

        // Command for running test
        RunDnsTest = ReactiveCommand.Create(
            () =>
            {
                // Notify the UI that a benchmark is in progress
                BenchmarkInProgress = true;

                // Reset task fields
                BenchmarkTasks.Clear();
                CompletedTaskCounter = 0;

                // Use half of the users resources to run the test
                ThreadPool.GetMaxThreads(out var workerThreads, out var completionPortThreads);
                ThreadPool.SetMinThreads(workerThreads / 2, completionPortThreads / 2);

                // Create a task for each nameserver test
                foreach (var nameserver in Nameservers)
                    BenchmarkTasks.Add(BenchmarkNameserver(nameserver, pingBenchmark));
            });

        // Rx property observers
        // When the selected nameserver is changed, update the plot
        this.WhenAnyValue(x => x.SelectedNameserver)
            .ObserveOn(RxApp.MainThreadScheduler).Skip(1).Subscribe(UpdateSelectedNameserver);

        // When the completed task counter is changed, update the list view
        this.WhenAnyValue(x => x.CompletedTaskCounter)
            .Throttle(TimeSpan.FromMilliseconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(UpdateListView);
    }

    // Properties
    public MainWindowViewModel MainViewModel { get; set; }
    public List<Task> BenchmarkTasks { get; set; }

    public object Lock { get; set; }

    // Commands
    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }


    // Live UI properties
    public ObservableCollection<NameserverViewModel>? Nameservers
    {
        get => _nameservers;
        set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }

    public NameserverViewModel? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    public bool BenchmarkInProgress
    {
        get => _benchmarkInProgress;
        set => this.RaiseAndSetIfChanged(ref _benchmarkInProgress, value);
    }

    public int CompletedTaskCounter
    {
        get => _completedTaskCounter;
        set => this.RaiseAndSetIfChanged(ref _completedTaskCounter, value);
    }


    // Methods
    // Update the nameserver list in the UI
    private void UpdateListView(int completedTaskCounter)
    {
        if (BenchmarkTasks.Count != 0)
        {
            if (completedTaskCounter >= BenchmarkTasks.Count)
            {
                BenchmarkInProgress = false;
                BenchmarkTasks.Clear();
                completedTaskCounter = 0;
            }

            // Sort collection and filter out poor quality nameservers
            if (Nameservers != null)
                Nameservers =
                    new ObservableCollection<NameserverViewModel>(Nameservers.OrderBy(o => o.AveragePing)
                        .Where(p => p.LatestPing != 0));
        }
    }

    // Run a benchmark test on the nameserver
    private Task BenchmarkNameserver(NameserverViewModel nameserver, IBenchmark pingBenchmark)
    {
        var task = Task.Run(() =>
        {
            for (var j = 0; j < 5; j++)
            {
                var ping = pingBenchmark.Run(nameserver.IpAddress);

                // Set the reply as the latest ping
                nameserver.LatestPing = ping;

                // Check if the server did not reply and return
                if (ping == 0) break;

                // Add the reply to the list of pings
                nameserver.Pings.Add(ping);

                nameserver.ObservablePings.Add(new ObservableValue(ping));

                // Calculate the average ping so it can be displayed in the view
                nameserver.CalculateAveragePing();

                nameserver.UpdateStatusIcon();
            }
        }).ContinueWith(t =>
        {
            lock (Lock)
            {
                CompletedTaskCounter++;
            }
        });

        return task;
    }

    public void UpdateSelectedNameserver(NameserverViewModel? selectedNameserver)
    {
        if (MainViewModel.LiveChartViewModel != null) MainViewModel.LiveChartViewModel.CreateSeries(selectedNameserver);


        MainViewModel.SettingsPanelViewModel.SelectedNameserver = selectedNameserver?.IpAddress;
    }
}