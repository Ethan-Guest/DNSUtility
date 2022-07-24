using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNSUtility.Domain.AppModels;
using DNSUtility.Domain.UserModels;
using DNSUtility.Service.Benchmarks;
using LiveChartsCore.Defaults;
using ReactiveUI;

namespace DNSUtility.Ui.ViewModels;

public class NameserverListViewModel : ViewModelBase
{
    // Private backing fields
    private readonly UserSettings _userSettings;
    private bool _benchmarkInProgress;
    private int _completedTaskCounter;
    private ObservableCollection<NameserverViewModel>? _nameservers;
    private string _selectedCountry;
    private NameserverViewModel? _selectedNameserver;

    // Constructor takes in a list of nameserver models, the benchmark service, and a reference to the main ViewModel TODO: switch the ping benchmark to the selected benchmark type from user settings. (Quick, Standard)
    public NameserverListViewModel(MainWindowViewModel mainWindowViewModel, IEnumerable<Nameserver> nameservers,
        IBenchmark pingBenchmark, UserSettings userSettings)
    {
        // Initialize reference to main view model
        MainViewModel = mainWindowViewModel;

        // Initialize user settings reference
        _userSettings = userSettings;

        // Initialize selected country
        SelectedCountry = userSettings.Country;

        // Initialize the list of nameservers for the UI
        Nameservers = new ObservableCollection<NameserverViewModel>();

        // Create a NameserverViewModel for each nameserver and add to observable collection
        foreach (var nameserver in nameservers)
        {
            var nameserverViewModel = new NameserverViewModel(nameserver);
            Nameservers.Add(nameserverViewModel);
        }

        // Collect all country codes and store in list
        CountryCodesList = Enum.GetNames(typeof(CountryInfo.CountryCodes)).ToList();
        CountryCodesList.Sort();

        // List of tasks to be run during test
        BenchmarkTasks = new List<Task>();

        // Shared lock object for thread safe task completion counter
        Lock = new object();

        // Counter to store the number of completed tasks
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
                    BenchmarkTasks.Add(BenchmarkNameserver(nameserver, pingBenchmark, 5));
            });

        // Rx property observers
        // When the selected nameserver is changed, update the plot
        this.WhenAnyValue(x => x.SelectedNameserver)
            .ObserveOn(RxApp.TaskpoolScheduler).Skip(1).Subscribe(UpdateSelectedNameserver);

        // When the selected country is changed, update the list with the selected country
        this.WhenAnyValue(x => x.SelectedCountry)
            .ObserveOn(RxApp.TaskpoolScheduler).Skip(1).Subscribe(UpdateSelectedCountry);

        // When the completed task counter is changed, update the list view
        this.WhenAnyValue(x => x.CompletedTaskCounter)
            .Throttle(TimeSpan.FromMilliseconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(UpdateListView);
    }

    // Properties
    //
    // The main view model reference
    public MainWindowViewModel MainViewModel { get; set; }

    // A list of tasks used in the benchmark test
    public List<Task> BenchmarkTasks { get; set; }

    // A lock object for thread safe task counting
    public object Lock { get; set; }

    // A list of strings to be displayed in a combobox
    public List<string> CountryCodesList { get; set; }

    // A reactive command for running the test
    public ReactiveCommand<Unit, Unit> RunDnsTest { get; set; }

    // Live UI properties
    //
    // The observable collection of nameservers
    public ObservableCollection<NameserverViewModel>? Nameservers
    {
        get => _nameservers;
        set => this.RaiseAndSetIfChanged(ref _nameservers, value);
    }

    // The currently selected nameserver in the list (data grid selection) TODO: Add support for multi-selection
    public NameserverViewModel? SelectedNameserver
    {
        get => _selectedNameserver;
        set => this.RaiseAndSetIfChanged(ref _selectedNameserver, value);
    }

    // The selected country
    public string SelectedCountry
    {
        get => _selectedCountry;
        set => this.RaiseAndSetIfChanged(ref _selectedCountry, value);
    }

    // A bool storing whether the benchmark is still in progress, used for UI progress bar and notifying UI controls
    public bool BenchmarkInProgress
    {
        get => _benchmarkInProgress;
        set => this.RaiseAndSetIfChanged(ref _benchmarkInProgress, value);
    }

    // A counter for completed tasks, used to check if all tasks have completed
    public int CompletedTaskCounter
    {
        get => _completedTaskCounter;
        set => this.RaiseAndSetIfChanged(ref _completedTaskCounter, value);
    }

    // Methods
    //
    // Update the nameserver list in the UI
    private void UpdateListView(int completedTaskCounter)
    {
        if (BenchmarkTasks.Count != 0)
        {
            // If the completed task counter is greater than or equal to the number of benchmark tasks being run, the test is over
            if (completedTaskCounter >= BenchmarkTasks.Count)
            {
                // Reset properties
                BenchmarkInProgress = false;
                BenchmarkTasks.Clear();
                completedTaskCounter = 0;
            }

            // Sort the collection of nameservers and filter out poor quality nameservers TODO: Instead of creating a new list each time (extremely inefficient) sort the list and notify the collection to refresh in the UI
            if (Nameservers != null)
                Nameservers =
                    new ObservableCollection<NameserverViewModel>(Nameservers.OrderBy(o => o.AveragePing)
                        .Where(p => p.LatestPing != 0));
        }
    }

    // Run a benchmark test on the nameserver
    private Task BenchmarkNameserver(NameserverViewModel nameserver, IBenchmark pingBenchmark, int numberOfTestsToRun)
    {
        var task = Task.Run(() =>
        {
            // Run 5 tests
            for (var j = 0; j < numberOfTestsToRun; j++)
            {
                // Ping the nameserver
                var ping = pingBenchmark.Run(nameserver.IpAddress);

                // Set the reply as the latest ping
                nameserver.LatestPing = ping;

                // Check if the server did not reply and return
                if (ping == 0) break;

                // Add the reply to the list of pings
                nameserver.Pings.Add(ping);

                // Add the ping to the live chart pings
                nameserver.ObservablePings.Add(new ObservableValue(ping));

                // Calculate the average ping so it can be displayed in the view
                nameserver.CalculateAveragePing();

                // Update the status icon used in UI
                nameserver.UpdateStatusIcon();
            }
        }).ContinueWith(t =>
        {
            // When the task has completed, increment the thread safe counter
            lock (Lock)
            {
                CompletedTaskCounter++;
            }
        });
        return task;
    }

    // Update the selected nameserver
    public void UpdateSelectedNameserver(NameserverViewModel? selectedNameserver)
    {
        // Update the livechart series with the selected nameserver
        MainViewModel.LiveChartViewModel.CreateSeries(selectedNameserver);

        // Update the selected nameserver in the livechart viewmodel
        MainViewModel.LiveChartViewModel.SelectedNameserver = selectedNameserver;
    }

    // Update the selected country
    public void UpdateSelectedCountry(string selectedCountry)
    {
        // Apply to settings
        _userSettings.Country = selectedCountry;

        // Recreate the list with the selected country
        MainViewModel.InitializeNameserverList();
    }
}